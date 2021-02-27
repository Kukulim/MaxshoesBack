using MaxshoesBack.Controllers;
using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MaxshoesBackTests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IUserServices> _mockRepo;
        private readonly EmployeeController _controller;
        public EmployeeControllerTests()
        {
            _mockRepo = new Mock<IUserServices>();
            _controller = new EmployeeController(_mockRepo.Object);


        }

        [Fact]
        public void GetEmployees_ActionExecutes_ReturnsOkObjectResult()
        {
            var result = _controller.GetEmployees();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetEmployees_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            _mockRepo.Setup(repo => repo.GetAllEmployee())
                .Returns(new List<User>() { new User { Role = UserRoles.Employee }, new User { Role = UserRoles.Employee } });

            var result = _controller.GetEmployees();

            OkObjectResult okResult = result as OkObjectResult;
            List<User> messages = okResult.Value as List<User>;
            Assert.Equal(2, messages.Count);
        }

        [Fact]
        public void CreateEmployee_ActionExecutes_ReturnsOk()
        {

            var result = _controller.CreateEmployee(new User
            {
                Id = "37846734-172e-4149-8cec-6f43d1eb3f60",
                UserName = "Employee1",
                IsEmailConfirmed = true,
                Email = "Employee1@test.pl",
                Password = "Employee1",
                Role = UserRoles.Employee,
                Contact = new Contact()
            }); ;
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            _mockRepo.Setup(r => r.Create(It.IsAny<User>()));

            _controller.ModelState.AddModelError("Password", "Password is required");
            var employee = new User { Contact = new Contact() };
            _controller.CreateEmployee(employee);
            _mockRepo.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void Create_ModelStateValid_CreateEmployeeCalledOnce()
        {
            _mockRepo.Setup(r => r.Create(It.IsAny<User>()));

            var employee = new User
            {
                UserName = "Test Employee",
                Email = "test@test.pl",
                Password = "123-5435789603-21",
                Contact = new Contact()
            };
            _controller.CreateEmployee(employee);
            _mockRepo.Verify(x => x.Create(It.IsAny<User>()), Times.Once);

        }

        [Fact]
        public void EditEmployee_ActionExecutes_ReturnsOkObject()
        {

            var result = _controller.EditEmployee(new User
            {
                Id = "37846734-172e-4149-8cec-6f43d1eb3f60",
                UserName = "Employee1",
                IsEmailConfirmed = true,
                Email = "Employee1@test.pl",
                Password = "Employee1",
                Role = UserRoles.Employee,
                Contact = new Contact()
            }); ;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Edit_ModelStateValid_EditEmployeeCalledOnce()
        {
            _mockRepo.Setup(r => r.EditEmployee(It.IsAny<User>()));

            var employee = new User
            {
                UserName = "Test Employee",
                Email = "test@test.pl",
                Password = "123-5435789603-21",
            };
            _controller.EditEmployee(employee);
            _mockRepo.Verify(x => x.EditEmployee(It.IsAny<User>()), Times.Once);
        }
    }
}
