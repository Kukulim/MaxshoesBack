using MaxshoesBack.Controllers;
using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.EmailService;
using MaxshoesBack.Services.NotificationServices;
using MaxshoesBack.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MaxshoesBackTests.Controllers
{
    public class NotificationControllerTests
    {
        private readonly Mock<INotificationServices> _mockNotificationRepo;
        private readonly Mock<IUserServices> _mockUserRepo;
        private readonly Mock<IEmailService> _mockEmailRepo;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _mockNotificationRepo = new Mock<INotificationServices>();
            _mockUserRepo = new Mock<IUserServices>();
            _mockEmailRepo = new Mock<IEmailService>();
            _controller = new NotificationController(_mockNotificationRepo.Object, _mockUserRepo.Object, _mockEmailRepo.Object);
        }

        [Fact]
        public void GetAllNotifications_ActionExecutes_ReturnsOkObjectResult()
        {
            var result = _controller.GetAllNotifications();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllNotifications_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            _mockNotificationRepo.Setup(repo => repo.GetAll())
                .Returns(new List<Notification>() { new Notification(), new Notification(), new Notification() });

            var result = _controller.GetAllNotifications();

            OkObjectResult okResult = result as OkObjectResult;
            List<Notification> messages = okResult.Value as List<Notification>;
            Assert.Equal(3, messages.Count);
        }

        [Fact]
        public void CreateNotification_ActionExecutes_ReturnsOk()
        {
            var result = _controller.CreateNotification(new Notification());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CreateNotification_ModelStateValid_CreateNotificationCalledOnce()
        {
            Notification noti = null;
            _mockNotificationRepo.Setup(r => r.Create(It.IsAny<Notification>()))
                .Callback<Notification>(x => noti = x);
            var notification = new Notification
            {
                Id = "fzs4bt4esatse4t4-5b4-4b5y3",
                Title = "title",
                Description = "Description"
            };
            _controller.CreateNotification(notification);

            _mockNotificationRepo.Verify(x => x.Create(It.IsAny<Notification>()), Times.Once);

            Assert.Equal(noti.Id, notification.Id);
            Assert.Equal(noti.Title, notification.Title);
            Assert.Equal(noti.Description, notification.Description);
        }
    }
}