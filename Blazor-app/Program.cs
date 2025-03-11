using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp1;
using shared.Model;
using kreddit_app.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Tilføj HttpClient som scoped service
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BaseApiUrl"] ?? "http://localhost:5283/") });

// Registrer ApiService som scoped
builder.Services.AddScoped<ApiService>();  

await builder.Build().RunAsync();
