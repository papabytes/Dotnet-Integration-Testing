namespace Dotnet.Template.UnitTests.Application.CQSR.Persons.Commands;

using Core.Entities;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Template.Application.CQSR.Persons.Commands;
using Template.Application.CQSR.Persons.Dtos;

public class CreatePersonCommandTests
{
    private Mock<ILogger<CreatePersonCommand>> _loggerMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    
    public CreatePersonCommandTests()
    {
        this._loggerMock = new();
        this._personRepositoryMock = new();
    }

    [Fact]
    public async Task ExecuteAsync_PersonDtoIsInvalid_ThrowsArgumentException()
    {
        // Arrange
        var personDto = new PersonDto();

        var service = this.GetTestService();
        
        // Act and Assert
        await Assert.ThrowsAsync<AggregateException>(async () => await service.ExecuteAsync(personDto));
    }

    [Fact]
    public async Task ExecuteAsync_PersonDtoIsValid_RepositoryThrowsException_ThrowsException()
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
        
        this._personRepositoryMock.Setup(_ => _.CreatePersonAsync(It.IsAny<Person>())).ThrowsAsync(new Exception());
        var service = this.GetTestService();
        
        // Act and Assert
        await Assert.ThrowsAsync<Exception>(async() => await service.ExecuteAsync(personDto));
    }
    
    [Fact]
    public async Task ExecuteAsync_PersonDtoIsValid_RepositorySavesItIntoDatabase_ReturnsPersonId()
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
        
        this._personRepositoryMock.Setup(_ => _.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(personDto.CitizenId);
        var service = this.GetTestService();
        
        // Act
        var personDbId = await service.ExecuteAsync(personDto);
        
        // Assert
        Assert.NotEqual(Guid.Empty, personDbId);
    }
    
    private CreatePersonCommand GetTestService()
    {
        return new CreatePersonCommand(this._loggerMock.Object, this._personRepositoryMock.Object);
    }
}
