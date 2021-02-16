using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.AccountModels;
using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.UserServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace MaxshoesBack.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        //private readonly IEmailService _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(IUserServices userService, IJwtAuthManager jwtAuthManager, IConfiguration configuration)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            //_emailSender = emailSender;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            return Ok("test");
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }

            if (!_userService.IsAnExistingUser(request.UserName, request.Email))
            {
                var newUser = new User
                {
                    UserName = request.UserName,
                    Password = BC.HashPassword(request.Password),
                    Email = request.Email
                };
                _userService.Create(newUser);

                var confirmEmail = new ConfirmEmailRequest { UserEmail = request.Email, UserName = request.UserName };
                await SendConfirmEmail(confirmEmail);

                return Ok(newUser);
            }

            return BadRequest("Email or User name is in use.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var CurrentUser = _userService.GetUserByEmail(request.Email);

            if (CurrentUser == null || !BC.Verify(request.Password, CurrentUser.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,CurrentUser.UserName),
                new Claim(ClaimTypes.Email, CurrentUser.Email)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(CurrentUser.UserName, claims, DateTime.Now);
            return Ok(new LoginResult
            {
                UserId = CurrentUser.Id,
                UserName = CurrentUser.UserName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString,
                Email = CurrentUser.Email,
                IsEmailConfirmed = CurrentUser.IsEmailConfirmed,
            });
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                UserName = User.Identity.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                OriginalUserName = User.FindFirst("OriginalUserName")?.Value
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            return Ok();
        }

        [HttpPost("remove")]
        [Authorize]
        public ActionResult Remove([FromBody] RemoveAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var UserToRemove = new User { UserName = request.UserName, Password = request.Password, Email = request.UserEmail };
            _userService.Delete(UserToRemove);
            return Ok();
        }

        [Authorize]
        [HttpPost("sendConfirmEmail")]
        public async Task<ActionResult> SendConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Email, request.UserEmail)
            };
            var ConfirmToken = _jwtAuthManager.GenerateConfirmEmailToken(request.UserName, claims, DateTime.Now);

            string Url = $"{_configuration["appUrl"]}/api/account/confirmemail?UserName={request.UserName}&token={ConfirmToken}";

            //await _emailSender.SendEmailAsync(request.UserEmail, "Confirm Email - ReactApp", "<h1>Hello from React Web</h1>" + $"<p> please confirm email by <a href='{Url}'>Click here!</a></p>");

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public ActionResult ConfirmEmailToken(string UserName, string token)
        {
            var AcceptedEmail = _jwtAuthManager.ConfirmEmailToken(UserName, token, DateTime.Now);
            if (AcceptedEmail == null)
            {
                return BadRequest();
            }
            var CurrentUser = _userService.GetUserByEmail(AcceptedEmail);
            CurrentUser.IsEmailConfirmed = true;
            _userService.Edit(CurrentUser);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("SendPasswordResetEmail")]
        public async Task<ActionResult> SendPasswordResetEmail([FromBody] PasswordResetRequest request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, request.Email)
            };
            var ConfirmToken = _jwtAuthManager.GeneratePasswordResetToken(claims, DateTime.Now);

            string Url = $"{_configuration["appUrl"]}/api/account/passwordreset?UserEmail={request.Email}&token={ConfirmToken}";

            //await _emailSender.SendEmailAsync(request.Email, "Reset Password - ReactApp", "<h1>Hello from React Web</h1>" + $"<p> to reset your password: <a href='{Url}'>Click here!</a></p>");

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("PasswordReset")]
        public async Task<ActionResult> PasswordReset(string UserEmail, string token)
        {
            var CurrentUser = _userService.GetUserByEmail(UserEmail);
            var AcceptedEmail = _jwtAuthManager.ConfirmPasswordResetToken(UserEmail, token);
            if (AcceptedEmail == null)
            {
                return BadRequest();
            }
            if (CurrentUser.Email == UserEmail)
            {
                CurrentUser.Password = _jwtAuthManager.GenerateTemporaryPasswordString();
                //await _emailSender.SendEmailAsync(UserEmail, "Reset Password - ReactApp", $"Your new temporary password: '{CurrentUser.Password}'");
                _userService.Edit(CurrentUser);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity.Name;

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }

        //[HttpPost("impersonation")]
        //[Authorize(Roles = UserRoles.Admin)]
        //public ActionResult Impersonate([FromBody] ImpersonationRequest request)
        //{
        //    var userName = User.Identity.Name;
        //    _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

        //    var impersonatedRole = _userService.GetUserRole(request.UserName);
        //    if (string.IsNullOrWhiteSpace(impersonatedRole))
        //    {
        //        _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
        //        return BadRequest($"The target user [{request.UserName}] is not found.");
        //    }
        //    if (impersonatedRole == UserRoles.Admin)
        //    {
        //        _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
        //        return BadRequest("This action is not supported.");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,request.UserName),
        //        new Claim(ClaimTypes.Role, impersonatedRole),
        //        new Claim("OriginalUserName", userName)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = request.UserName,
        //        Role = impersonatedRole,
        //        OriginalUserName = userName,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}

        //[HttpPost("stop-impersonation")]
        //public ActionResult StopImpersonation()
        //{
        //    var userName = User.Identity.Name;
        //    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
        //    if (string.IsNullOrWhiteSpace(originalUserName))
        //    {
        //        return BadRequest("You are not impersonating anyone.");
        //    }
        //    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

        //    var role = _userService.GetUserRole(originalUserName);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,originalUserName),
        //        new Claim(ClaimTypes.Role, role)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = originalUserName,
        //        Role = role,
        //        OriginalUserName = null,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}
    }
}