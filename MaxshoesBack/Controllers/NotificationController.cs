using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace MaxshoesBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationServices _notificationServices;

        public NotificationController(INotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
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
        public ActionResult EditNotification([FromBody] Notification request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _notificationServices.Edit(request);
            _notificationServices.Complete();
            return Ok();
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

                    return Ok(new { dbPath });
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


    }
}