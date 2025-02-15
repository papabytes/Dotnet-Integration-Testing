namespace Dotnet.Template.IntegrationTests.Application.CQRS.Persons.Commands;

using Template.Application.CQSR.Persons.Dtos;

public class CreatePersonCommandIntegrationTests : IClassFixture<CreatePersonCommandIntegrationTestFixture>
{
    private readonly CreatePersonCommandIntegrationTestFixture _fixture;

    public CreatePersonCommandIntegrationTests(CreatePersonCommandIntegrationTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public async Task ExecuteAsync_StoresPersonOnDb()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            Country = "POR",
            DateOfBirth = new DateTime(1995, 10, 22),
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };

        var service = this._fixture.GetTestService();

        // Act
        var res = await service.ExecuteAsync(personDto);

        // Assert
        Assert.NotEqual(Guid.Empty, res);
    }
}
