using MaxshoesBack;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MaxshoesBackIntegrationTests.Controllers
{
    public class EmployeeControlerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;
        public EmployeeControlerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient();
        }
        [Fact]
        public async Task GetEmployees_ReturnsUnauthorized()
        {
            var response = await httpClient.GetAsync("/api/employee/getallemployee");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
