using MaxshoesBack.Controllers;
using MaxshoesBack.Services.EmailService;
using MaxshoesBack.Services.NotificationServices;
using MaxshoesBack.Services.UserServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxshoesBackTests.Controllers
{
    class NotificationControllerTests
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
            _controller = new NotificationController(_mockNotificationRepo.Object,_mockUserRepo.Object, _mockEmailRepo.Object);


        }
    }
}
