using DataAccess.Context;
using MeetUp.EShop.Presentation.Handlers;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using MeetUp.EShop.Tests.TestApiContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetUp.EShop.Presentation.CircuitServicesAccesor;
using MeetUp.EShop.Core.Interfaces;
using MeetUp.EShop.Business.Reposirories;
using Microsoft.Extensions.Configuration;
using MeetUp.EShop.Tests.RequestHandlers;
using MeetUp.EShop.Tests.AuthContext.Interfaces;
using MeetUp.EShop.Tests.AuthContext.Implementations;

namespace MeetUp.EShop.Tests.DependencyInjection
{
    public class DependencyInjectionSetUpper
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();


            var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory) // або Directory.GetCurrentDirectory()
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                    .Build();

            // Реєстрація IConfiguration
            services.AddSingleton<IConfiguration>(configuration);


            services.AddDbContext<EShopDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<ITestAuthContext, TestAuthContext>();
            services.AddScoped<TestAuthHandler>();

            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer()
            };
            services.AddRefitClient<IUserAPI>(refitSettings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://localhost:7025");
                })
                .AddHttpMessageHandler<TestAuthHandler>()
                ;
            
            
            services.AddRefitClient<IAuthAPI>(refitSettings)
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://localhost:7025");
            });

            return services;
        }
    }
}
