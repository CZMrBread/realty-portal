using Client;
using Client.Features.RealtyAgency;
using Client.Features.RealtyAgent;
using Client.Features.SRealty;
using Client.Features.User;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var serverApi = new Uri(builder.Configuration["ServerAPI"]!);

// each slice registers what it is made of, the way the server maps its own endpoints
builder.Services.AddUserFeature(serverApi);
builder.Services.AddRealtyAgencyFeature(serverApi);
builder.Services.AddRealtyAgentFeature(serverApi);
builder.Services.AddSRealtyFeature(serverApi);

await builder.Build().RunAsync();
