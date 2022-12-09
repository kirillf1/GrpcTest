using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IPersonRepository
    {
        public Task<Persons.Person> GetPersonByName<Person>(string name);
        public Task<List<Person>> GetAllPersons();
        public Task<Person> CreatePerson(string name,string password,int? roleId);
    }
}
