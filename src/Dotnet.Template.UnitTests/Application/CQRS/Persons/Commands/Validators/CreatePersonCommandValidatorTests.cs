namespace Dotnet.Template.UnitTests.Application.CQSR.Persons.Commands.Validators;

using Template.Application.CQSR.Persons.Commands.Validators;
using Template.Application.CQSR.Persons.Dtos;

public class CreatePersonCommandValidatorTests
{
    [Fact]
    public void Validate_AllPersonDtoFieldsAreNull_ReturnsAggregateException()
    {
        // Arrange
        var person = new PersonDto();
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(person);

        // Assert
        Assert.NotNull(aggrEx);
        Assert.Equal(6, aggrEx.InnerExceptions.Count);
    }

    [Fact]
    public void Validate_PersonDtoCitizenIdIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            Country = "POR",
            DateOfBirth = new DateTime(1995, 10, 22),
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    
    [Fact]
    public void Validate_PersonDtoCountryIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = new DateTime(1995, 10, 22),
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    
    [Fact]
    public void Validate_PersonDtoDateOfBirthIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            Country = "POR",
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    
    [Fact]
    public void Validate_PersonDtoFirstNameIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = new DateTime(1995,10,22),
            Country = "POR",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    
    [Fact]
    public void Validate_PersonDtoLastNameIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = new DateTime(1995,10,22),
            Country = "POR",
            FirstName = "Antonio",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    
    [Fact]
    public void Validate_PersonDtoSocialSecurityNumberIsNull_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = new DateTime(1995,10,22),
            Country = "POR",
            FirstName = "Antonio",
            LastName = "Guedes"
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    [Fact]
    public void Validate_PersonDtoDateTimeIsFuture_ReturnsAggregateExceptionWithSingleInnerException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = DateTime.Today.AddDays(1),
            Country = "POR",
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Single(aggrEx.InnerExceptions);
    }
    
    [Fact]
    public void Validate_PersonDtoIsOk_ReturnsNullAggregateException()
    {
        // Arrange
        var personDto = new PersonDto
        {
            CitizenId = Guid.NewGuid().ToString(),
            DateOfBirth = new DateTime(1995,10,22),
            Country = "POR",
            FirstName = "Antonio",
            LastName = "Guedes",
            SocialSecurityNumber = new Random().Next(100_000_000, 300_000_000).ToString()
        };
        
        // Act
        var aggrEx = CreatePersonCommandValidator.Validate(personDto);
        
        // Assert
        Assert.Null(aggrEx);
    }
}
