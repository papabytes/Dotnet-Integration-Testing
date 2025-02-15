namespace Dotnet.Template.Application.CQSR.Persons.Commands.Validators;

using Dtos;

public class CreatePersonCommandValidator
{
    public static AggregateException? Validate(PersonDto personDto)
    {
        var exceptions = new List<Exception>();
        if (string.IsNullOrWhiteSpace(personDto.CitizenId))
        {
            exceptions.Add(new ArgumentNullException($"{nameof(personDto.CitizenId)} is required and must not be empty."));
        }

        if (string.IsNullOrWhiteSpace(personDto.FirstName))
        {
            exceptions.Add(new ArgumentException($"{nameof(personDto.FirstName)} is required and must not be empty."));
        }

        if (string.IsNullOrWhiteSpace(personDto.LastName))
        {
            exceptions.Add(new ArgumentException($"{nameof(personDto.LastName)} is required and must not be empty."));
        }

        if (personDto.DateOfBirth == null)
        {
            exceptions.Add(new ArgumentNullException($"{personDto.DateOfBirth} is required."));
        }
        else
        {
            var now = DateTime.UtcNow;
            if (personDto.DateOfBirth > now)
            {
                exceptions.Add(new ArgumentException($"{personDto.DateOfBirth} cannot be in the future."));
            }
        }

        if (string.IsNullOrWhiteSpace(personDto.SocialSecurityNumber))
        {
            exceptions.Add(new ArgumentNullException($"{personDto.SocialSecurityNumber} is required."));
        }

        if (string.IsNullOrWhiteSpace(personDto.Country))
        {
            exceptions.Add(new ArgumentNullException($"{personDto.Country} is required."));
        }


        if (exceptions.Count > 0)
        {
            return new AggregateException(exceptions);
        }

        return null;
    }
}
