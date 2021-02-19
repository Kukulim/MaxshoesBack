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
    }
}
