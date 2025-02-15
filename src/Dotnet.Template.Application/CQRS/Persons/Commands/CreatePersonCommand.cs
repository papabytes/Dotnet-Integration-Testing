namespace Dotnet.Template.Application.CQSR.Persons.Commands;

using Converters;
using Core.Entities;
using Core.Repositories;
using Dtos;
using Microsoft.Extensions.Logging;
using Validators;

public interface ICreatePersonCommand : ICommand<Guid, PersonDto>;

public class CreatePersonCommand  : ICreatePersonCommand
{
    private readonly ILogger<CreatePersonCommand> _logger;
    private readonly IPersonRepository _personRepository;

    public CreatePersonCommand(ILogger<CreatePersonCommand> logger, IPersonRepository personRepository)
    {
        this._logger = logger;
        this._personRepository = personRepository;
    }

    public async Task<Guid> ExecuteAsync(PersonDto personDto)
    {
        var exception = CreatePersonCommandValidator.Validate(personDto);
        if (exception != null)
        {
            throw exception;
        }

        var converter = new ModelConverterBase<PersonDto, Person>();
        var personDb = converter.Convert(personDto);
        personDb.Id = Guid.NewGuid();

        try
        {
            await this._personRepository.CreatePersonAsync(personDb);
            this._logger.LogInformation("[CreatePersonCommand]: Created Person with Citizen Id = '{citizenId}'.", personDb.CitizenId);
            return personDb.Id;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "[CreatePersonCommand]: An error occurred while saving Person with citizen id {citizenId} to the database.", personDb.CitizenId);
            throw;
        }
    }
}
