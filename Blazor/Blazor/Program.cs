using Application;
using Blazor.Client.Services;
using Blazor.Components;
using Blazor.Middleware;
using Infrastructure;
using MudBlazor.Services;
using Serilog;

namespace Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                    .Build())
                .CreateLogger();

            try
            {
                Log.Information("Starting Blazor application with integrated API");

                var builder = WebApplication.CreateBuilder(args);

                // Add Serilog
                builder.Host.UseSerilog();

                // Add MudBlazor services
                builder.Services.AddMudServices();

                // Configure HttpClient for API (same origin, no BaseAddress needed)
                builder.Services.AddScoped<HttpClient>();
                builder.Services.AddScoped<ApiService>();

                // Add API services
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddApplication(builder.Configuration);
                
                var connectionString = builder.Configuration.GetConnectionString("Default")
                                         ?? throw new ArgumentException("Connection string 'Default' not found");
                builder.Services.AddInfrastructure(connectionString);

                // Add Blazor services
                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents()
                    .AddInteractiveWebAssemblyComponents();

                var app = builder.Build();

                // Log configured URLs
                var configuredUrls = builder.Configuration["ASPNETCORE_URLS"];
                if (string.IsNullOrEmpty(configuredUrls))
                {
                    configuredUrls = builder.Configuration.GetSection("Kestrel:Endpoints").Exists() 
                        ? "Kestrel configuration" 
                        : "http://localhost:5000 (default)";
                }
                Log.Information("Configured URLs: {ConfiguredUrls}", configuredUrls);

                // Add Serilog request logging
                app.UseSerilogRequestLogging(options =>
                {
                    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress);
                        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
                    };
                });

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseWebAssemblyDebugging();
                    app.UseSwagger();
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
                    });
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                // Add validation exception middleware
                app.UseMiddleware<ValidationExceptionMiddleware>();

                app.UseAntiforgery();

                app.MapStaticAssets();
                
                // Map API controllers
                app.MapControllers();
                
                // Map Blazor components
                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode()
                    .AddInteractiveWebAssemblyRenderMode()
                    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

                // Log the URLs the application is listening on
                var urls = app.Urls.Any() ? string.Join(", ", app.Urls) : "default (http://localhost:5000)";
                Log.Information("Blazor application with integrated API started successfully");
                Log.Information("Application is listening on: {Urls}", urls);
                
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
