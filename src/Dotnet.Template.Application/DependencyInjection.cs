namespace Dotnet.Template.Application;

using CQSR.Persons.Commands;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICreatePersonCommand, CreatePersonCommand>();
    }
}
