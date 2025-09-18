using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis")
    .WithRedisCommander()
    .WithDataVolume();

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("eshopdb", "MeetUp.EShop.DB");


var api = builder.AddProject<Projects.MeetUp_EShop_Api>("eshopapi")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(db)
    .WaitFor(db);

var ui = builder.AddProject<Projects.MeetUp_EShop_Presentation>("eshopui")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
