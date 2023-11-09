using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Authentications.Models;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTest
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;
        public static HttpClient Client;
        public static AppDbContext DbContext;
        public static ISender Mediator;

        [OneTimeSetUp]
        public void RunBeforeAnyTest()
        {
            var application = new CustomWebApplicationFactory<Program>();
            _scopeFactory = application.Services.GetRequiredService<IServiceScopeFactory>();
            _configuration = application.Services.GetRequiredService<IConfiguration>();
            DbContext = application.Services.GetRequiredService<AppDbContext>();
            Mediator = application.Services.GetRequiredService<ISender>();
            Client = application.CreateClient();

            DbContext.Database.EnsureCreated();
            ConfigHttpClient();
        }
        
        public static async Task RunAsMaster()
        {
            await LoginAndAttachToken("master@gmail.com", "123123");
        }
        
        public static async Task RunAsAdmin()
        {
            await LoginAndAttachToken("admin@gmail.com", "123123");
        }
        
        public static async Task RunAsDefaultUser()
        {
            await LoginAndAttachToken("user@gmail.com", "123123");
        }

        private void ConfigHttpClient()
        {
            Client.BaseAddress = new Uri("http://localhost/api/");
        }

        private static async Task LoginAndAttachToken(string username, string password)
        {
            var res = await Client.PostAsJsonAsync("auth/login", new { username, password });
            var auth = await res.Content.ReadFromJsonAsync<AuthenticationViewModel>();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTest()
        {
            DbContext.Database.EnsureDeleted();
        }
    }
}