using Client;
using Client.Features.RealtyAgent;
using Client.Features.User;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var serverApi = new Uri(builder.Configuration["ServerAPI"]!);

// each slice registers what it is made of, the way the server maps its own endpoints
builder.Services.AddUserFeature(serverApi);
builder.Services.AddRealtyAgentFeature();

await builder.Build().RunAsync();
