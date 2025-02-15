namespace Dotnet.Template.IntegrationTests.Application.CQRS.Persons.Commands;

using Template.Application.CQSR.Persons.Commands;

[Collection("GlobalTestCollection")]
public class CreatePersonCommandIntegrationTestFixture : TestFixtureBase
{
    public override async Task InitializeAsync()
    {
        await base.StartContainerAsync(this.MongoDbContainer);
    }

    public ICreatePersonCommand GetTestService()
    {
        return this.GetService<ICreatePersonCommand>();
    }
}
