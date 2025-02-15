namespace Dotnet.Template.Infrastructure;

using Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class MongoDbConnection
{
    private readonly ILogger<MongoDbConnection> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _databaseName;
    private readonly string _connectionString;
    private IMongoDatabase? _database;
    private readonly MongoClient _client;

    public MongoDbConnection(ILogger<MongoDbConnection> logger, IConfiguration configuration)
    {
        this._logger = logger;
        this._configuration = configuration;

        var connectionString =
            this._configuration.GetValue<string>(MongoDbInfrastructureEnvironmentVariables.MongoDBConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            this._logger.LogCritical(
                "Environment variable {environmentVariable} is not set and MongoDb Connection cannot be established. Exitting.",
                MongoDbInfrastructureEnvironmentVariables.MongoDBConnectionString);
            Environment.Exit(1);
        }

        var databaseName =
            this._configuration.GetValue<string>(MongoDbInfrastructureEnvironmentVariables.MongoDBDatabase);
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            this._logger.LogCritical(
                "Environment variable {environmentVariable} ist not set and MongoDb Connection cannot be established. Exiting.",
                MongoDbInfrastructureEnvironmentVariables.MongoDBDatabase);
            Environment.Exit(1);
        }

        _connectionString = connectionString;
        _databaseName = databaseName;
        _client = new MongoClient(connectionString);
    }

    public IMongoDatabase Database
    {
        get
        {
            if (_database == null)
            {
                this._database = this._client.GetDatabase(this._databaseName);
            }

            return this._database;
        }
    }
}
