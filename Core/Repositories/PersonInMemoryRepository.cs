using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        public Task<Person> CreatePerson(string name, string password, int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Person>> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPersonByName<Person>(string name)
        {
            throw new NotImplementedException();
        }
        
    }
}
