using Dets.Services;
using DetsLife;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ISavingService, SaveOnDevice>();
builder.Services.AddSingleton<HistoryService>();

var host = builder.Build();
var js = host.Services.GetRequiredService<Microsoft.JSInterop.IJSRuntime>();
try { await js.InvokeVoidAsync("eval", "navigator.storage && navigator.storage.persist && navigator.storage.persist()"); } catch { }
await host.RunAsync();
