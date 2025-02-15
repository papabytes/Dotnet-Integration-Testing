namespace Dotnet.Template.Infrastructure.Repositories;

using System.Linq.Expressions;
using Constants;
using Core.Entities;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
    protected override string CollectionName => CollectionNames.Persons;
    public PersonRepository(ILogger<RepositoryBase<Person>> logger, MongoDbConnection mongoDbConnection) : base(logger, mongoDbConnection)
    {
    }
    
    public async Task<string> CreatePersonAsync(Person person)
    {
        await Collection.InsertOneAsync(person);
        return person.CitizenId;
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync(Expression<Func<Person, bool>> clause)
    {
        var filter = Builders<Person>.Filter.Where(clause); 
        return await Collection.Find(filter).ToListAsync();
    }

    public Task<Person?> GetSinglePersonAsync(Expression<Func<Person, bool>> clause)
    {
        var filter = Builders<Person>.Filter.Where(clause);
        return Collection.Find(filter).SingleOrDefaultAsync();
    }

    public Task<Person?> GetPersonByCitizenIdAsync(string citizenId)
    {
        var filter = Builders<Person>.Filter.Eq(p => p.CitizenId, citizenId);
        return Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdatePersonAsync(string citizenId, Person updatedData)
    {
        // Doing blind replacement
        var filter = Builders<Person>.Filter.Eq(p => p.CitizenId, citizenId);
        var res = await Collection.ReplaceOneAsync(filter, updatedData);
        return res.ModifiedCount > 0;
    }
}
