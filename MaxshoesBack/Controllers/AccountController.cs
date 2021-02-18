using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.AccountModels;
using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.EmailService;
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
        private readonly IEmailService _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(IUserServices userService, IJwtAuthManager jwtAuthManager, IEmailService emailSender, IConfiguration configuration)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpGet("user")]
        public ActionResult GetCurrentUser([FromBody] LoginRequest request)
        {
            var CurrentUser = _userService.GetUserByEmail(request.Email);
            return Ok(CurrentUser);
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
                    Email = request.Email,
                    Contact = new Contact()
                };
                _userService.Create(newUser);
                _userService.Complete();

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

            if (request.Email==_configuration["ShopOwner:Email"] && request.Password == _configuration["ShopOwner:Password"])
            {
                CurrentUser = new User
                {
                    Id = "maxshopowner",
                    Email = request.Email,
                    Password = BC.HashPassword(request.Password),
                    Role = UserRoles.MaxShopOwner,
                    UserName = "Max",
                    IsEmailConfirmed = true
                };
            }

            if (CurrentUser == null || !BC.Verify(request.Password, CurrentUser.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,CurrentUser.UserName),
                new Claim(ClaimTypes.Email, CurrentUser.Email),
                new Claim(ClaimTypes.Role, CurrentUser.Role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(CurrentUser.UserName, claims, DateTime.Now);
            return Ok(new LoginResult
            {
                UserId = CurrentUser.Id,
                Role = CurrentUser.Role,
                UserName = CurrentUser.UserName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString,
                Email = CurrentUser.Email,
                IsEmailConfirmed = CurrentUser.IsEmailConfirmed,
                Contact = CurrentUser.Contact,
                Notifications = CurrentUser.Notifications
            }) ;
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {

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
            _userService.Complete();
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

            await _emailSender.SendEmailAsync(request.UserEmail, "Confirm Email - Maxshoes", "<h1>Hello from Max Shoes</h1>" + $"<p> please confirm email by <a href='{Url}'>Click here!</a></p>");

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
            _userService.Complete();
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

            await _emailSender.SendEmailAsync(request.Email, "Reset Password - Maxshoes", "<h1>Hello from Max shoes</h1>" + $"<p> to reset your password: <a href='{Url}'>Click here!</a></p>");

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
                await _emailSender.SendEmailAsync(UserEmail, "Reset Password - Maxshoes", $"Your new temporary password: '{CurrentUser.Password}'");
                _userService.Edit(CurrentUser);
                _userService.Complete();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var CurrentUser = _userService.GetUserByEmail(request.Email);
            if (BC.Verify(request.OldPassword, CurrentUser.Password))
            {
                CurrentUser.Password = BC.HashPassword(request.NewPassword);
                _userService.Edit(CurrentUser);
                _userService.Complete();
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
        [HttpPost("editcontact")]
        [Authorize]
        public ActionResult EditContact([FromBody] EditContactRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var UserToEdit = _userService.GetUserByEmail(request.Email);
            UserToEdit.Contact = request.Contact;
            _userService.Edit(UserToEdit);
            _userService.Complete();
            return Ok();
        }

    }
}