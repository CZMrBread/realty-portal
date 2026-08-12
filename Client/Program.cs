using Client;
using Client.Features.User;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uri = new Uri(builder.Configuration["ServerAPI"]!);

builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped(sp => new HttpClient(
    new BearerTokenHandler(sp.GetRequiredService<CookieService>())
    {
        InnerHandler = new HttpClientHandler()
    })
{
    BaseAddress = uri
});
builder.Services.AddScoped<AuthStateService>();

await builder.Build().RunAsync();
