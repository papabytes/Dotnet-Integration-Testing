namespace Dotnet.Template.UnitTests.Application;

using Core.Entities;
using Template.Application.Converters;
using Template.Application.CQSR.Persons.Dtos;

public class ModelConverterTests
{
    [Fact]
    public void ModelConverter_Convert_ConvertsTwoTypesSuccessfully()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            CitizenId = Guid.NewGuid().ToString(),
            Country = "POR",
            DateOfBirth = new DateTime(1994, 8, 22),
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        var converter = new ModelConverterBase<PersonDto, Person>();
        
        // Act
        var personDto = converter.Convert(person);
        
        // Assert
        Assert.Equal(personDto.CitizenId, person.CitizenId);
        Assert.Equal(personDto.FirstName, person.FirstName);
        Assert.Equal(personDto.LastName, person.LastName);
        Assert.Equal(personDto.DateOfBirth, person.DateOfBirth);
    }
}
