using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccess.Context
{
    public class EShopDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
    {
        public EShopDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MeetUp.EShop")) // шлях до appsettings.json
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();

            var dbType = configuration.GetValue<DataBaseType>("DbType");

            switch (dbType)
            {
                case DataBaseType.SqlServer:
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("SQLServer"));
                    break;
                case DataBaseType.InMemory:
                default:
                    optionsBuilder.UseInMemoryDatabase("EShopDb");
                    break;
            }

            return new EShopDbContext(optionsBuilder.Options);
        }
    }
}
