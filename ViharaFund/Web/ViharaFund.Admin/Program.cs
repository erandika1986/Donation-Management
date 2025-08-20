using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ViharaFund.Admin;
using ViharaFund.Admin.Handlers;
using ViharaFund.Admin.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

builder.Services.AddScoped<LoadingService>();
builder.Services.AddAuthorizationCore();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7044") });
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    var handler = new CustomAuthorizationMessageHandler(navigation)
    {
        InnerHandler = new HttpClientHandler()
    };

    return new HttpClient
    {
        //BaseAddress = new Uri("https://localhost:7044")
        BaseAddress = new Uri(" https://api.donationmanager.xyz")
    };

});




builder.Services.AddMudServices();
await builder.Build().RunAsync();
