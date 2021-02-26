using MaxshoesBack;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MaxshoesBackIntegrationTests.HealthCheck
{
    public class HealthCheckTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;
        public HealthCheckTest(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient();
        }
        [Fact]
        public async Task HealthCheck_ReturnOK()
        {
            var response = await httpClient.GetAsync("/healthcheck");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
