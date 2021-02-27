using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult CreateEmployee([FromBody] User request)
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
            var editedEmployee = _userService.EditEmployee(request);
            _userService.Complete();
            return Ok(editedEmployee);
        }
    }
}