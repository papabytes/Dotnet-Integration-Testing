namespace Dotnet.Template.Infrastructure;

using Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using Repositories;

public static class DependencyInjection
{
    public static void AddMongoDbInfrastructureServices(this IServiceCollection services)
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention()
        };
        ConventionRegistry.Register("camelCase", conventionPack, t => true);
        var objectSerializer = new ObjectSerializer(type => ObjectSerializer.AllAllowedTypes(type));

        // NOTE: Do not remove this - this if is majorly important for integration tests
        if (BsonSerializer.SerializerRegistry.GetSerializer(typeof(ObjectSerializer)) == null)
        {
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        var guidSerializer = BsonSerializer.SerializerRegistry.GetSerializer(typeof(GuidSerializer));
        if (guidSerializer == null)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
        else
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }

        services.AddSingleton<MongoDbConnection>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddSingleton<MongoDbInitializer>();
    }

    public static async Task MigrateDatabaseAsync(this WebApplication webApplication)
    {
        var scope = webApplication.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetService<MongoDbInitializer>();
        await initializer.InitializeAsync();
    }
}
