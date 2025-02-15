namespace Dotnet.Template.Api.Constants;

public class ApiEnvironmentVariables
{
    public const string WebServerHostingUrls = "ASPNETCORE_URLS";
    public const string WebServerCors = "CORS";

    public class OpenTelemetry
    {
        public const string ServiceName = "OTEL_SERVICE_NAME";
    }
}
