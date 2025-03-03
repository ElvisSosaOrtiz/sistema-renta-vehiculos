using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Microsoft.AspNetCore.Components.Authorization;
using Client.LocalServices;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["APIBaseUrl"]!) })
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore()
    .AddScoped<CustomAuthStateProvider>()
    .AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>())
    .AddScoped<AuthService>();

await builder.Build().RunAsync();
