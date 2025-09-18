using MeetUp.EShop.Presentation.CircuitServicesAccesor;
using MeetUp.EShop.Presentation.Components;
using MeetUp.EShop.Presentation.Handlers;
using MeetUp.EShop.Presentation.Services;
using MeetUp.EShop.Presentation.Services.Authorization;
using MeetUp.EShop.Presentation.Services.Inteerfaces;
using MeetUp.EShop.Presentation.Services.Product;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddLocalization();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, EShopAuthStateProvider>();

builder.Services.AddScoped<TokenHandler>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddCircuitServicesAccesor();
builder.Services.AddServiceDiscovery();


var refitSettings = new RefitSettings
{
    ContentSerializer = new NewtonsoftJsonContentSerializer()
};
var apiUrl = new Uri("https://eshopapi");
builder.Services.AddRefitClient<IUserAPI>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = apiUrl;
    })
    .AddServiceDiscovery()
    .AddHttpMessageHandler<TokenHandler>();
builder.Services.AddRefitClient<IProductAPI>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = apiUrl;
    })
    .AddServiceDiscovery()
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddRefitClient<IOrderAPI>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = apiUrl;
    })
    .AddServiceDiscovery()
    .AddHttpMessageHandler<TokenHandler>()
    .AddHttpMessageHandler(() => new LoggingHandler());

builder.Services.AddRefitClient<IAuthAPI>(refitSettings)
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = apiUrl;
    })
    .AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

var supportedCultures = app.Configuration.GetSection("SupportedCultures").Get<string[]>();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[1])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
