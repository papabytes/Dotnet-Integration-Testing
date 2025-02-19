namespace Dotnet.Template.Api.Constants;

public class ApiEnvironmentVariables
{
    public const string WebServerHostingUrls = "ASPNETCORE_URLS";
    public const string WebServerCors = "CORS";

    public class Logging
    {
        public const string MinimumLogLevel = "MINIMUM_LOG_LEVEL";
    }

    public class OpenTelemetry
    {
        public const string ServiceName = "OTEL_SERVICE_NAME";
    }
}
