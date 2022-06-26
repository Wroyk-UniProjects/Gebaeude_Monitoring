using Building_Monitoring_WebApp;
using Building_Monitoring_WebApp.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IConfigService, ConfigService>();

var app = builder.Build();



await app.RunAsync();
