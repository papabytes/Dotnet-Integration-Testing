namespace Dotnet.Template.Application.CQSR.Persons.Dtos;

public class PersonDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SocialSecurityNumber { get; set; }
    public string CitizenId { get; set; }
    public string Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
