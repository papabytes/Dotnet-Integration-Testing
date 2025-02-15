namespace Dotnet.Template.IntegrationTests;

using Api;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

[Collection("GlobalTestCollection")]
public abstract class TestFixtureBase : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const int MongoHostPort = 27018;
    private const int MongoDbPort = 27017;
    
    public IContainer MongoDbContainer = new ContainerBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(MongoHostPort, MongoDbPort)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MongoDbPort))
        .Build();

    private List<IContainer> _runningContainers = [];
    private IServiceScope? _scope;

    public abstract Task InitializeAsync();

    public virtual async Task StartContainerAsync(IContainer container)
    {
        await container.StartAsync();
        this._runningContainers.Add(container);
    }

    public T GetService<T>()
    {
        if (this._scope != null) return this._scope.ServiceProvider.GetService<T>();
        
        this._scope = this.Services.CreateScope();

        return _scope.ServiceProvider.GetService<T>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        SetEnvironmentVariables();
        base.ConfigureWebHost(builder);
    }

    private void SetEnvironmentVariables()
    {
         Environment.SetEnvironmentVariable(MongoDbInfrastructureEnvironmentVariables.MongoDBDatabase, $"mongodb://localhost:{MongoHostPort}");
         Environment.SetEnvironmentVariable(MongoDbInfrastructureEnvironmentVariables.MongoDBDatabase, "dotnet_template");
    }

    public virtual async Task DisposeAsync()
    {
        if (this._runningContainers.Count > 0)
        {
            await Task.WhenAll(this._runningContainers.Select(async c => c.StopAsync()));
        }
    }
}
