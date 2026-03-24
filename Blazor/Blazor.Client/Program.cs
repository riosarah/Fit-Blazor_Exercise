using Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace Blazor.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            // Configure MudBlazor with PopoverOptions
            builder.Services.AddMudServices(config =>
            {
                config.PopoverOptions.ThrowOnDuplicateProvider = false;
            });

            // Configure HttpClient for API (empty = same origin)
            var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "";
            
            builder.Services.AddScoped(sp => 
            {
                var httpClient = new HttpClient 
                { 
                    BaseAddress = string.IsNullOrEmpty(apiBaseUrl) 
                        ? new Uri(builder.HostEnvironment.BaseAddress) 
                        : new Uri(apiBaseUrl) 
                };
                return httpClient;
            });
            
            builder.Services.AddScoped<ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
