using MaxshoesBack;
using MaxshoesBack.Services.NotificationServices;
using MaxshoesBack.Services.UserServices;
using MaxshoesBackIntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace MaxshoesBackIntegrationTests
{
    public class TestHostFixture : IDisposable
    {
        public HttpClient Client { get; }
        public IServiceProvider ServiceProvider { get; }

        public FakeUserDataBase FakeUserDataBase { get; }
        public FakeNotyficationDataBase FakeNotyficationDataBase { get; set; }

        public TestHostFixture()
        {
            FakeUserDataBase = FakeUserDataBase.WithDefaultUsers();
            FakeNotyficationDataBase = FakeNotyficationDataBase.WithDefaultNotifications();

            var builder = Program.CreateHostBuilder(null)
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseEnvironment("Test");
                    webHost.ConfigureTestServices(services =>
                    {
                        services.AddSingleton<IUserServices>(FakeUserDataBase);
                        services.AddSingleton<INotificationServices>(FakeNotyficationDataBase);
                    });
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