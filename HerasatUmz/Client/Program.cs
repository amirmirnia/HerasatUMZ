using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Services;
using Client.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Domain.Enum;
using Client.Services.Interface;
using Client.Services.Repository;
using Client.Services.Alert;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddTransient<IVisitLogger, VisitLogger>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<AlertService>();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("Role", nameof(UserRole.Admin)));

    options.AddPolicy("Manager", policy =>
        policy.RequireClaim("Role", nameof(UserRole.Manager)));

    options.AddPolicy("User", policy =>
        policy.RequireClaim("Role", nameof(UserRole.User)));

    options.AddPolicy("AdminOrManager", policy =>
        policy.RequireClaim("Role", nameof(UserRole.Admin), nameof(UserRole.Manager)));

    options.AddPolicy("AdminOrManagerOrUser", policy =>
        policy.RequireClaim("Role", nameof(UserRole.Admin), nameof(UserRole.Manager), nameof(UserRole.User)));
});

builder.Services.AddTransient<AuthDelegatingHandler>();

builder.Services
    .AddHttpClient("HerasatUmz.ServerAPI",
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AuthDelegatingHandler>();

builder.Services.AddHttpClient("HerasatUmz.NoAuth",
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("HerasatUmz.ServerAPI"));

await builder.Build().RunAsync();
