namespace Dotnet.Template.Infrastructure;

using Constants;
using Core.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class MongoDbInitializer
{
    private readonly ILogger<MongoDbInitializer> _logger;
    private readonly MongoDbConnection _connection;

    public MongoDbInitializer(ILogger<MongoDbInitializer> logger, MongoDbConnection connection)
    {
        this._logger = logger;
        this._connection = connection;
    }

    public async Task InitializeAsync()
    {
       await CreateUniqueIndexForPersonCollectionAsync();
    }

    private async Task CreateUniqueIndexForPersonCollectionAsync()
    {
        var personCollection = this._connection.Database.GetCollection<Person>(CollectionNames.Persons);
    
        var citizenIdKeys = Builders<Person>.IndexKeys.Ascending(p => p.CitizenId);
        var citizenIdIndexOptions = new CreateIndexOptions { Unique = true };
        var citizenIdIndexModel = new CreateIndexModel<Person>(citizenIdKeys, citizenIdIndexOptions);

        await personCollection.Indexes.CreateOneAsync(citizenIdIndexModel);
    }
}
