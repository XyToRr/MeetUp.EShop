using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlServer");
var sql = builder.AddConnectionString("sql-server");

var api = builder.AddProject<Projects.MeetUp_EShop_Api>("eshopapi")
    .WithReference(sql);

var ui = builder.AddProject<Projects.MeetUp_EShop_Presentation>("eshopui")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
