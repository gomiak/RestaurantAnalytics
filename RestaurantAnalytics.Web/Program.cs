using DotNetEnv;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using RestaurantAnalytics.Application.Services;
using RestaurantAnalytics.Core.Interfaces;
using RestaurantAnalytics.Infrastructure;
using RestaurantAnalytics.Web.Auth;
using RestaurantAnalytics.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IAiInsightsService, AiInsightsService>();



//Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();
builder.Services.AddMudBlazorSnackbar();
builder.Services.AddMudBlazorResizeObserver();



//Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

Env.Load();
builder.Configuration.AddEnvironmentVariables();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? throw new Exception("DB_CONNECTION not found.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddAuthentication("cookies")
    .AddCookie("cookies", options =>
    {
        options.LoginPath = "/login";
    });

builder.Services.AddScoped<SimpleAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<SimpleAuthStateProvider>());
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
//The default HSTS value is 30 days. You may want to change this for production scenarios, see https:aka.ms / aspnetcore - hsts.
app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();


app.Run();
