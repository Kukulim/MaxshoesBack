using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
