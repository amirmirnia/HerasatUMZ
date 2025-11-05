using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Domain.Enum;
using Client.Services.Interface;
using Client.Services.Repository;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddTransient<IVisitLogger, VisitLogger>();
builder.Services.AddScoped<UserContextService>();

builder.Services.AddAuthorizationCore(options =>
{
    // مثال: Policy برای ادمین‌ها
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("role", nameof(UserRole.Admin)));

    options.AddPolicy("Manager", policy =>
        policy.RequireClaim("role", nameof(UserRole.Manager)));

    options.AddPolicy("User", policy =>
        policy.RequireClaim("role", nameof(UserRole.User)));

    options.AddPolicy("AdminOrManager", policy =>
    policy.RequireClaim("role", nameof(UserRole.Admin), nameof(UserRole.Manager)));

    options.AddPolicy("AdminOrManagerOrPaymentUser", policy =>
policy.RequireClaim("role", nameof(UserRole.Admin), nameof(UserRole.Manager), nameof(UserRole.PaymentUser)));

});


builder.Services.AddHttpClient("HerasatUmz.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("HerasatUmz.ServerAPI"));

await builder.Build().RunAsync();
