using DataAccess.Context;
using MeetUp.EShop.Business.Cache.Interfaces;
using MeetUp.EShop.Tests.TestServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeetUp.EShop.Tests.ApiFactory
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {

            builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.FullName);
            //builder.UseUrls("http://localhost:5001");

            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var testSettingsPath = Path.Combine(
                    Directory.GetCurrentDirectory(), // це буде шлях до тестового проєкту
                    "appsettings.test.json"
                );

                configBuilder.AddJsonFile(testSettingsPath, optional: false);
            });



            builder.ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                var cacheDescriptor = services.FirstOrDefault(
                    d => d.ServiceType == typeof(IHybridCacheService));

                if (cacheDescriptor != null)
                {
                    services.Remove(cacheDescriptor);
                }

                // додати нашу тестову реалізацію
                services.AddScoped<IHybridCacheService, TestCacheService>();

                var contextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<EShopDbContext>));

                if (contextDescriptor != null)
                    services.Remove(contextDescriptor);

                var connectionString = configuration.GetConnectionString("SQLServer");

                services.AddDbContext<EShopDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            });

        }
        
    }
}
