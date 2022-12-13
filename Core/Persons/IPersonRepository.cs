using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    public interface IPersonRepository
    {
        public Task<Person> GetPersonByName(string name);
        public Task<List<Person>> GetAllPersons();
        public Task<Person> CreatePerson(string name, string password, int? roleId);
    }
}
