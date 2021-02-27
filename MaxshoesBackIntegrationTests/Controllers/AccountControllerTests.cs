using MaxshoesBack.Controllers;
using MaxshoesBack.JwtAuth;
using MaxshoesBack.Models.UserModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace MaxshoesBackIntegrationTests.Controllers
{
    public class AccountControllerTests
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

        public AccountControllerTests()
        {
            _httpClient = _testHostFixture.Client;
            _serviceProvider = _testHostFixture.ServiceProvider;
        }
        [Fact]
        public async Task ShouldExpect401WhenLoginWithInvalidCredentials()
        {
            var credentials = new LoginRequest
            {
                Email = "invalidEmail",
                Password = "invalidPassword"
            };
            var response = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task ShouldReturnCorrectResponseForSuccessLogin()
        {
            var credentials = new LoginRequest
            {
                Email = "Employee1@test.pl",
                Password = "Employee1"
            };
            var response = await _httpClient.PostAsync("api/account/login",
                new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, MediaTypeNames.Application.Json));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task ShouldReturnCorrectResponseForSuccessMaxLogin()
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

            Assert.Equal(credentials.Email, loginResult.Email);
            Assert.Equal(UserRoles.MaxShopOwner, loginResult.Role);
            Assert.False(string.IsNullOrWhiteSpace(loginResult.AccessToken));
            Assert.False(string.IsNullOrWhiteSpace(loginResult.RefreshToken));

            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var (principal, jwtSecurityToken) = jwtAuthManager.DecodeJwtToken(loginResult.AccessToken);
            Assert.Equal(UserRoles.MaxShopOwner, principal.FindFirst(ClaimTypes.Role).Value);
            Assert.NotNull(jwtSecurityToken);
        }
        [Fact]

        public async Task MaxShouldBeAbleToLogout()
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
            var logoutResponse = await _httpClient.PostAsync("api/account/logout", null);
            Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
            Assert.False(jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.ContainsKey(loginResult.RefreshToken));
        }

        [Fact]
        public async Task ShouldCorrectlyRefreshToken()
        {
            const string userName = "Employee1";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, UserRoles.Employee)
            };
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var jwtResult = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-1));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwtResult.AccessToken);
            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
            var response = await _httpClient.PostAsync("api/account/refresh-token",
                new StringContent(JsonConvert.SerializeObject(refreshRequest), Encoding.UTF8, MediaTypeNames.Application.Json));
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResult>(responseContent);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var refreshToken2 = jwtAuthManager.UsersRefreshTokensReadOnlyDictionary.GetValueOrDefault(result.RefreshToken);
            Assert.Equal(refreshToken2.TokenString, result.RefreshToken);
            Assert.NotEqual(refreshToken2.TokenString, jwtResult.RefreshToken.TokenString);
            Assert.NotEqual(jwtResult.AccessToken, result.AccessToken);
        }


        [Fact]
        public async Task ShouldNotAllowToRefreshTokenWhenRefreshTokenIsExpired()
        {
            const string userName = "Employee1";
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, UserRoles.Employee)
            };
            var jwtAuthManager = _serviceProvider.GetRequiredService<IJwtAuthManager>();
            var jwtTokenConfig = _serviceProvider.GetRequiredService<JwtTokenConfig>();
            var jwtResult1 = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-jwtTokenConfig.RefreshTokenExpiration - 1));
            var jwtResult2 = jwtAuthManager.GenerateTokens(userName, claims, DateTime.Now.AddMinutes(-1));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwtResult2.AccessToken); 
            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = jwtResult1.RefreshToken.TokenString
            };
            var response = await _httpClient.PostAsync("api/account/refresh-token",
                new StringContent(JsonConvert.SerializeObject(refreshRequest), Encoding.UTF8, MediaTypeNames.Application.Json));
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Invalid token", responseContent);
        }
    }
}
