namespace Dotnet.Template.Infrastructure;

using Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

public static class DependencyInjection
{
    public static void AddMongoDbInfrastructureServices(this IServiceCollection services)
    {
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
