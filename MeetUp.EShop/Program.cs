using MeetUp.EShop.Business.Reposirories;
using MeetUp.EShop.Business.Services;
using MeetUp.EShop.Core.Interfaces;
using Moq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MeetUp.EShop.Business.Interfaces;
using MeetUp.EShop.Business.Helpers;
using Microsoft.AspNetCore.Authorization;
using MeetUp.EShop.Filter;
using MeetUp.EShop.Api.Middlewares;
using Serilog;
using Serilog.Sinks.File;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataSeed;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.log",
    rollingInterval: RollingInterval.Day,
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level:u3}] {Message:lj}{NewLine}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter ONLY token."
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ITokenGenerator, AccessTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var key = Encoding.ASCII.GetBytes(builder.Configuration["Token"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


var connection = builder.Configuration.GetConnectionString("sql-server");
builder.Services.AddDbContext<EShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-server"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EShopDbContext>();
    if(context.Database.IsRelational())
        context.Database.Migrate();

#if DEBUG
    DataSeeder.Seed(context);
#endif
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionCatcher>();
app.MapControllers();

app.Run();
Log.CloseAndFlush();


