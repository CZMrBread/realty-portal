using Client;
using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var uri = new Uri(builder.Configuration["ServerAPI"]!);
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = uri });
builder.Services.AddScoped<AuthService>();
await builder.Build().RunAsync();