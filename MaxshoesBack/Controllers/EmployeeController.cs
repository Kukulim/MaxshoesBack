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
    [Authorize(Roles = UserRoles.MaxShopOwner)]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserServices _userService;

        public EmployeeController(IUserServices userService)
        {
            _userService = userService;
        }


        [HttpGet("getallemployee")]
        public ActionResult GetEmployees()
        {
            var Employees = _userService.GetAllEmployee();
            return Ok(Employees);
        }

        [HttpPost("createemployee")]
        public ActionResult Login([FromBody] User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            request.Password = BC.HashPassword(request.Password);
            request.Contact.UserId = request.Id;
            _userService.Create(request);
            _userService.Complete();
            return Ok();
   
        }

        [HttpPost("editemployee")]
        public ActionResult EditEmployee([FromBody] User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _userService.EditEmployee(request);
            _userService.Complete();
            return Ok();
        }
    }
}