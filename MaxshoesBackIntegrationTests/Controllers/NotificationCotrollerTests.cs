using MaxshoesBack.Controllers;
using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.UserModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MaxshoesBackIntegrationTests.Controllers
{
    public class NotificationCotrollerTests
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private HttpClient _httpClient;
        private IServiceProvider _serviceProvider;


        public NotificationCotrollerTests()
        {
            _httpClient = _testHostFixture.Client;
            _serviceProvider = _testHostFixture.ServiceProvider;
        }

        [Fact]
        public async Task GetAllNotifications_ReturnsUnauthorizedWhenNotLogin()
        {
            var response = await _httpClient.GetAsync("/api/notification/getall");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAllNotifications_ReturnsUnauthorizedForRoleDifrentThanEmployee()
        {
            var credentials = new LoginRequest
            {
                Email = "Test@test.pl",
                Password = "Test1234"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var logoutResponse = await _httpClient.GetAsync("api/notification/getall");
            Assert.Equal(HttpStatusCode.Forbidden, logoutResponse.StatusCode);
        }
        [Fact]
        public async Task GetAllNotifications_ReturnsSuccessForEmployeeRole()
        {
            var credentials = new LoginRequest
            {
                Email = "Employee2@test.pl",
                Password = "Employee2"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var logoutResponse = await _httpClient.GetAsync("api/notification/getall");
            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
        }
        [Fact]
        public async Task GetAllNotifications_ReturnsExpectedArrayOfNotificationsForEmployeeRole()
        {
            var credentials = new LoginRequest
            {
                Email = "Employee2@test.pl",
                Password = "Employee2"
            };

            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var getallnotificationResponse = await _httpClient.GetAsync("api/notification/getall");
            var getallnotifications = await getallnotificationResponse.Content.ReadAsStringAsync();
            var getallnotificationResult = JsonConvert.DeserializeObject<Notification[]>(getallnotifications);

            Assert.Equal(_testHostFixture.FakeNotyficationDataBase.Notifications.Count(), getallnotificationResult.Length);
        }

        [Fact]
        public async Task CreateNotification_NotCreateNotificationForRoleDifrentThanCustomer()
        {
            var credentials = new LoginRequest
            {
                Email = "Employee2@test.pl",
                Password = "Employee2"
            };

            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            var NotificationToCreate = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                Title = "newtitle",
                Description = "newdeccription",
                Response = "newresponse",
                CreatedAt = DateTime.Now,
                Status = Status.newNotify
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            await _httpClient.PostAsync("api/notification/createnotification", new StringContent(JsonConvert.SerializeObject(NotificationToCreate), Encoding.UTF8, MediaTypeNames.Application.Json));

            Assert.NotEqual(4, _testHostFixture.FakeNotyficationDataBase.Notifications.Count());
        }
        [Fact]
        public async Task CreateNotification_ReturnsExpectedNotificationResaultForCustomerRole()
        {
            _testHostFixture.FakeNotyficationDataBase.ResetDefaultNotifications();

            var credentials = new LoginRequest
            {
                Email = "Test@test.pl",
                Password = "Test1234"
            };

            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            var NotificationToCreate = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                Title = "newtitle",
                Description = "newdeccription",
                Response = "newresponse",
                CreatedAt = DateTime.Now,
                Status = Status.newNotify
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            await _httpClient.PostAsync("api/notification/createnotification", new StringContent(JsonConvert.SerializeObject(NotificationToCreate), Encoding.UTF8, MediaTypeNames.Application.Json));

            Assert.Equal(4, _testHostFixture.FakeNotyficationDataBase.Notifications.Count());
        }

        [Fact]
        public async Task EditNotification_ReturnsEditedEmployeeForMaxShopowner()
        {
            _testHostFixture.FakeNotyficationDataBase.ResetDefaultNotifications();

            var credentials = new LoginRequest
            {
                Email = "Employee2@test.pl",
                Password = "Employee2"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var orgilanNotificationTitle = _testHostFixture.FakeNotyficationDataBase.Notifications.Where(n => n.Id == "97846734-172e-4149-8cec-6f43d1eb3f61").FirstOrDefault().Title;
            var NotificationToEdit = _testHostFixture.FakeNotyficationDataBase.Notifications.Where(n => n.Id == "97846734-172e-4149-8cec-6f43d1eb3f61").FirstOrDefault();
            NotificationToEdit.Title = "newTitle";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);            

            var getallnotificationResponse = await _httpClient.PostAsync("api/notification/editnotification", new StringContent(JsonConvert.SerializeObject(NotificationToEdit), Encoding.UTF8, MediaTypeNames.Application.Json));
            var getallnotifications = await getallnotificationResponse.Content.ReadAsStringAsync();
            var getallnotificationResult = JsonConvert.DeserializeObject<Notification>(getallnotifications);

            var editedNotificationFromDataBase = _testHostFixture.FakeNotyficationDataBase.Notifications.Where(n => n.Id == "97846734-172e-4149-8cec-6f43d1eb3f61").FirstOrDefault();

            Assert.NotEqual(orgilanNotificationTitle, getallnotificationResult.Title);
        }
    }
}
