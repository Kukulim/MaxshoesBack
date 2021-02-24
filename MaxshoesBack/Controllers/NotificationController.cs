using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.EmailService;
using MaxshoesBack.Services.NotificationServices;
using MaxshoesBack.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MaxshoesBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notificationServices;
        private readonly IUserServices _userService;
        private readonly IEmailService _emailSender;

        public NotificationController(INotificationServices notificationServices, IUserServices userService, IEmailService emailSender)
        {
            _notificationServices = notificationServices;
            _userService = userService;
            _emailSender = emailSender;
        }

        [Authorize(Roles = UserRoles.Employee)]
        [HttpGet("getall")]
        public ActionResult GetAllNotifications()
        {
            var NotificationsList = _notificationServices.GetAll();
            return Ok(NotificationsList);
        }

        [Authorize(Roles = UserRoles.Customer)]
        [HttpPost("createnotification")]
        public ActionResult CreateNotification([FromBody] Notification request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid.");
            }
            _notificationServices.Create(request);
            _notificationServices.Complete();
            return Ok(request);
        }

        [HttpPost("editnotification")]
        [Authorize(Roles = UserRoles.Employee)]
        public async Task<ActionResult> EditNotification([FromBody] Notification request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var UserToSend = _userService.GetUserByID(request.UserId);

            await _emailSender.SendEmailAsync(UserToSend.Email, "The notification status has changed", "<h1>Hello from Max Shoes</h1>" + $"<p> The notification that id ={request.Id} status has changed</p> <p>Please check your account on our site.</p>");
            _notificationServices.Edit(request);
            _notificationServices.Complete();
            return Ok(request);
        }

        [Authorize]
        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Notifications");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileNameToInsert = Guid.NewGuid() + fileName;
                    var fullPath = Path.Combine(pathToSave, fileNameToInsert);
                    var dbPath = Path.Combine(folderName, fileNameToInsert);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { fileNameToInsert });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("DownloadFile/{name}")]
        public IActionResult Download(string name)
        {
            var path = @"E:/Nauka/Max/MaxshoesBack/MaxshoesBack/Resources/Notifications/" + name;
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "text/plain",Path.GetFileName(path));
        }
    }
}