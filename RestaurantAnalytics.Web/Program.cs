using DotNetEnv;
using MudBlazor.Services;
using RestaurantAnalytics.Infrastructure;
using RestaurantAnalytics.Web.Components;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? throw new Exception("DB_CONNECTION not found.");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
