namespace Dotnet.Template.Core.Repositories;

using System.Linq.Expressions;
using Entities;

public interface IPersonRepository
{
    Task<string> CreatePersonAsync(Person person);
    Task<IEnumerable<Person>> GetPersonsAsync(Expression<Func<Person, bool>> clause);
    Task<Person?> GetSinglePersonAsync(Expression<Func<Person, bool>> clause);
    Task<Person?> GetPersonByCitizenIdAsync(string citizenId);
    Task<bool> UpdatePersonAsync(string citizenId, Person updatedData);
}
