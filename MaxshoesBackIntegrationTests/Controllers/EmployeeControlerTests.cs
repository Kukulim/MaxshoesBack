using MaxshoesBack;
using MaxshoesBack.Controllers;
using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.UserModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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
    public class EmployeeControlerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly TestHostFixture _testHostFixture = new TestHostFixture();
        private HttpClient _httpClient;
        private IServiceProvider _serviceProvider;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        public EmployeeControlerTests()
        {
            _httpClient = _testHostFixture.Client;
            _serviceProvider = _testHostFixture.ServiceProvider;
        }
        [Fact]
        public async Task GetEmployees_ReturnsUnauthorizedWhenNotLogin()
        {
            var response = await _httpClient.GetAsync("/api/employee/getallemployee");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetEmployees_ReturnsUnauthorizedForRoleDifrentThanMaxShopowner()
        {
            var config = InitConfiguration();
            var credentials = new LoginRequest
            {
                Email = "Employee1@test.pl",
                Password = "Employee1"
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var logoutResponse = await _httpClient.GetAsync("api/employee/getallemployee");
            Assert.Equal(HttpStatusCode.Forbidden, logoutResponse.StatusCode);
        }

        [Fact]
        public async Task GetEmployees_ReturnsSuccessForMaxShopowner()
        {
            var config = InitConfiguration();
            var credentials = new LoginRequest
            {
                Email = config["ShopOwner:Email"],
                Password = config["ShopOwner:Password"]
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var logoutResponse = await _httpClient.GetAsync("api/employee/getallemployee");
            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
        }

        [Fact]
        public async Task GetEmployees_ReturnsExpectedArrayOfEmployeesForMaxShopowner()
        {
            var config = InitConfiguration();
            var credentials = new LoginRequest
            {
                Email = config["ShopOwner:Email"],
                Password = config["ShopOwner:Password"]
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var getallemployeeResponse = await _httpClient.GetAsync("api/employee/getallemployee");
            var getallemployee = await getallemployeeResponse.Content.ReadAsStringAsync();
            var getallEmployeeResult = JsonConvert.DeserializeObject<User[]>(getallemployee);

            Assert.Equal(_testHostFixture.FakeUserDataBase.Users.Where( u=>u.Role==UserRoles.Employee).Count(), getallEmployeeResult.Length);
        }

        [Fact]
        public async Task CreateEmployees_ReturnsExpectedArrayOfEmployeesForMaxShopowner()
        {
            var config = InitConfiguration();
            var credentials = new LoginRequest
            {
                Email = config["ShopOwner:Email"],
                Password = config["ShopOwner:Password"]
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));

            var UserToCreate = new User
            {
                Id = "37846734-172e-4149-8cec-6f43d1eb3f63",
                UserName = "TestCustomer",
                IsEmailConfirmed = true,
                Email = "Test@test.pl",
                Password = "Test1234",
                Role = UserRoles.Employee,
                Contact = new Contact()
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            await _httpClient.PostAsync("api/employee/createemployee", new StringContent(JsonConvert.SerializeObject(UserToCreate), Encoding.UTF8, MediaTypeNames.Application.Json));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var getallemployeeResponse = await _httpClient.GetAsync("api/employee/getallemployee");
            var getallemployee = await getallemployeeResponse.Content.ReadAsStringAsync();
            var getallEmployeeResult = JsonConvert.DeserializeObject<User[]>(getallemployee);

            _testHostFixture.FakeUserDataBase.ResetDefaultUsers(useCustomIfAvailable: false);

            Assert.Equal(3, getallEmployeeResult.Length);

        }
        [Fact]
        public async Task EditEmployee_ReturnsEditedEmployeeForMaxShopowner()
        {
            var config = InitConfiguration();
            var credentials = new LoginRequest
            {
                Email = config["ShopOwner:Email"],
                Password = config["ShopOwner:Password"]
            };
            var loginResponse = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(loginResponseContent);

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            Assert.True(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));


            var orgilanUserEmail = _testHostFixture.FakeUserDataBase.Users.Where(u => u.Id == "37846734-172e-4149-8cec-6f43d1eb3f60").FirstOrDefault().Email;
            var UserToEdit = _testHostFixture.FakeUserDataBase.Users.Where(u => u.Id == "37846734-172e-4149-8cec-6f43d1eb3f60").FirstOrDefault();
            UserToEdit.Email = "newEmailAdress";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, loginResult.AccessToken);
            var getemployeeResponse = await _httpClient.PostAsync("api/employee/editemployee", new StringContent(JsonConvert.SerializeObject(UserToEdit), Encoding.UTF8, MediaTypeNames.Application.Json));
            var getemployee = await getemployeeResponse.Content.ReadAsStringAsync();
            var getEmployeeResult = JsonConvert.DeserializeObject<User>(getemployee);

            Assert.NotEqual(orgilanUserEmail, getEmployeeResult.Email);
        }
    }
}
