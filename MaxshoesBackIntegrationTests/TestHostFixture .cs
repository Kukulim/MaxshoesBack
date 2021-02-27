using System;
using System.Net.Http;
using MaxshoesBack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.Json;
using MaxshoesBackIntegrationTests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using MaxshoesBack.Services.UserServices;

namespace MaxshoesBackIntegrationTests
{
    public class TestHostFixture : IDisposable
    {
        public HttpClient Client { get; }
        public IServiceProvider ServiceProvider { get; }

        public FakeUserDataBase FakeUserDataBase { get; }

        public TestHostFixture()
        {
            FakeUserDataBase = FakeUserDataBase.WithDefaultUsers();

            var builder = Program.CreateHostBuilder(null)
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseEnvironment("Test");
                    webHost.ConfigureTestServices(services =>
                        services.AddSingleton<IUserServices>(FakeUserDataBase));
                });


            var host = builder.Start();
            ServiceProvider = host.Services;
            Client = host.GetTestClient();
            Console.WriteLine("TEST Host Started.");
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
