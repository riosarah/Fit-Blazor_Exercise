using Application;
using Infrastructure;
using Serilog;

namespace Api
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
                Log.Information("Starting Books API application");

                var builder = WebApplication.CreateBuilder(args);

                // Add Serilog
                builder.Host.UseSerilog();

                // Add services to the container.

                // Add CORS for Blazor WebAssembly
                builder.Services.AddCors(options =>
                {
                    if (builder.Environment.IsDevelopment())
                    {
                        options.AddPolicy("AllowBlazorWasm", policy =>
                        {
                            policy.WithOrigins(
                                    "https://localhost:7075", 
                                    "http://localhost:5096",
                                    "https://localhost:7074", 
                                    "http://localhost:5095")
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials();
                        });
                    }
                    else
                    {
                        options.AddPolicy("AllowBlazorWasm", policy =>
                        {
                            policy.WithOrigins("https://localhost:7075", "http://localhost:5096")
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials();
                        });
                    }
                });

                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.AddApplication();
                var connectionString = builder.Configuration.GetConnectionString("Default")
                                         ?? throw new ArgumentException("Connection string 'Default' not found");
                builder.Services.AddInfrastructure(connectionString);


                var app = builder.Build();

                // Add Serilog request logging
                app.UseSerilogRequestLogging(options =>
                {
                    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress);
                        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                    };
                });

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseSwagger();
                    app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Books API V1");
                    });
                }

                app.UseHttpsRedirection();

                app.UseCors("AllowBlazorWasm");

                app.UseAuthorization();


                app.MapControllers();

                Log.Information("Books API application started successfully");
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
