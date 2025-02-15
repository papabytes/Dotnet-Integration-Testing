namespace Dotnet.Template.Infrastructure.Repositories;

using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public abstract class RepositoryBase<T>
{
    protected readonly ILogger<RepositoryBase<T>> Logger;
    private readonly MongoDbConnection _mongoDbConnection;

    private IMongoCollection<T>? _collection;

    protected abstract string CollectionName { get; }

    public RepositoryBase(ILogger<RepositoryBase<T>> logger, MongoDbConnection mongoDbConnection)
    {
        this.Logger = logger;
        this._mongoDbConnection = mongoDbConnection;
    }

    protected IMongoCollection<T> Collection
    {
        get
        {
            return this._collection ??= this._mongoDbConnection.Database.GetCollection<T>(this.CollectionName);
        }
    }
}
