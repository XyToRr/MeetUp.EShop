using DataAccess.Context;
using MeetUp.EShop.Presentation.Handlers;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using MeetUp.EShop.Tests.TestApiContext;
using System;
using MeetUp.EShop.Presentation.CircuitServicesAccesor;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Business.Reposirories;
using Microsoft.Extensions.Configuration;
using MeetUp.EShop.Tests.RequestHandlers;
using MeetUp.EShop.Tests.AuthContext.Interfaces;
using MeetUp.EShop.Tests.AuthContext.Implementations;
using Microsoft.EntityFrameworkCore;
using MeetUp.EShop.Tests.ApiFactory;

namespace MeetUp.EShop.Tests.DependencyInjection
{
    public class DependencyInjectionSetUpper
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                    .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<EShopDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("SQLServer");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<ITestAuthContext, TestAuthContext>();
            services.AddScoped<TestAuthHandler>();

            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            };

            // Запускаємо тестовий API через WebApplicationFactory
            var factory = new ApiFactory.ApiFactory();

            // ------------------------------
            // Підключаємо Refit через TestServer
            // ------------------------------
            services.AddRefitClient<IUserAPI>(refitSettings)
                .ConfigureHttpClient(c =>
                {
                    // будь-яка формальна адреса
                    c.BaseAddress = new Uri("http://localhost");
                })
                .ConfigurePrimaryHttpMessageHandler(() => factory.Server.CreateHandler())
                .AddHttpMessageHandler<TestAuthHandler>();

            services.AddRefitClient<IAuthAPI>(refitSettings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("http://localhost");
                })
                .ConfigurePrimaryHttpMessageHandler(() => factory.Server.CreateHandler());

            return services;
        }
    }
}
