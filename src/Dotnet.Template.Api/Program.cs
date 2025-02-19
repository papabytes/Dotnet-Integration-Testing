namespace Dotnet.Template.Api;

using Application;
using Constants;
using Infrastructure;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

public partial class Program
{
    public static async Task Main(params string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = ConfigureConfiguration(builder); 
        
        builder.Services.AddControllers();
        ConfigureKestrel(builder, configuration);
        ConfigureCors(builder, configuration);
        ConfigureServices(builder);
        ConfigureOpenTelemetry(builder, configuration);
        
        var app = builder.Build();
        app.MapControllers();
        app.UseCors();

        try
        {
            Console.WriteLine("[Startup]: Starting Application.");
            await app.MigrateDatabaseAsync();
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL]: An unhandled exception occurred {ex}.");
        }
    }

    private static void ConfigureOpenTelemetry(IHostApplicationBuilder hostBuilder, IConfiguration configuration)
    {
        var minimumLogLevelEnvVar = configuration.GetValue<LogLevel?>(ApiEnvironmentVariables.Logging.MinimumLogLevel) ?? LogLevel.Information;
        
        hostBuilder.Logging.SetMinimumLevel(minimumLogLevelEnvVar);
        hostBuilder.Logging.ClearProviders()
            .AddFilter("Microsoft", LogLevel.Error);


        string[] excludableEndpointPaths = ["/healthz"];
       
        
        hostBuilder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(opts =>
                    {
                        opts.Filter = (context) => excludableEndpointPaths.Contains(context.Request.PathBase.Value);
                    })
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter();
            })
            .WithLogging(configureBuilder =>
            {
                configureBuilder.AddConsoleExporter();
            }, configureOptions =>
            {
                configureOptions.IncludeScopes = true;
                configureOptions.IncludeFormattedMessage = true;
            });
    }

    /// <summary>
    /// Configures IConfiguration
    /// </summary>
    /// <param name="hostApplicationBuilder"></param>
    /// <returns></returns>
    private static IConfiguration ConfigureConfiguration(IHostApplicationBuilder hostApplicationBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        hostApplicationBuilder.Configuration.AddConfiguration(configuration);
        return configuration;
    }

    /// <summary>
    /// Configures the URLS that the server will listen at.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="webApplicationBuilder"></param>
    private static void ConfigureKestrel(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration)
    {
        const ushort defaultPort = 8080;
        var configuredUrls = configuration.GetValue<string>(ApiEnvironmentVariables.WebServerHostingUrls);
        if (string.IsNullOrWhiteSpace(configuredUrls))
        {
            var defaultUrls = $"http://+:{defaultPort}";
            Console.WriteLine(
                $"The hosting environment variable {ApiEnvironmentVariables.WebServerHostingUrls} was not set. Using default URLs: {defaultUrls}");
            webApplicationBuilder.WebHost.UseUrls(defaultUrls);
        }
        else
        {
            webApplicationBuilder.WebHost.UseUrls(configuredUrls);
        }
    }

    private static void ConfigureCors(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration)
    {
        webApplicationBuilder.Services.AddCors(act =>
        {
            act.AddPolicy("ApplicationCors", builder =>
            {
                builder = builder.AllowAnyMethod().AllowAnyHeader();

                var cors = configuration.GetValue<string>(ApiEnvironmentVariables.WebServerCors);
                if (string.IsNullOrWhiteSpace(cors))
                {
                    builder.AllowAnyOrigin().Build();
                    return;
                }

                // The environment variable should configure urls
                var urls = cors.Split(",");
                if (urls.Length == 0)
                {
                    Console.WriteLine("Found 0 CORS origin urls. Defaulting to allow any.");
                    builder.AllowAnyOrigin().Build();
                    return;
                }

                var validUrls = new List<string>();
                foreach (var url in urls)
                {
                    if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                    {
                        Console.WriteLine($"Found CORS Origin {url}. Registering.");
                        validUrls.Add(url);
                    }
                    else
                    {
                        Console.WriteLine($"Found invalid CORS origin {url}. Skipping.");
                    }
                }

                if (validUrls.Count > 0)
                {
                    builder.WithOrigins(validUrls.ToArray());
                    return;
                }

                Console.WriteLine(
                    "After processing the allowed origin urls, the total of valid urls was 0. Allowing all origins...");
                builder.AllowAnyOrigin();
            });
        });
    }

    private static void ConfigureHost(IHostApplicationBuilder hostApplicationBuilder)
    {
    }

    private static void ConfigureServices(IHostApplicationBuilder hostApplicationBuilder)
    {
        hostApplicationBuilder.Services.AddMongoDbInfrastructureServices();
        hostApplicationBuilder.Services.AddApplicationServices();
    }
}
