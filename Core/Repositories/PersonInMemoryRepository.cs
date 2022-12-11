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
        private static List<Person> persons = new List<Person>();
        private RoleInMemoryRepository roleInMemoryRepository = new();
        public async Task<Person> CreatePerson(string name, string password, int? roleId)
        {
            Role? role = default;
            if (roleId.HasValue) 
            {
                var roles = await roleInMemoryRepository.GetRole();
                role = roles.Find(c => c.Id == roleId);
            }
            var person = new Person(persons.Count +1, name, password, role);
            persons.Add(person);
            return person;
        }

        public Task<List<Person>> GetAllPersons()
        {
            return Task.FromResult(persons);
        }

        public Task<Persons.Person> GetPersonByName<Person>(string name)
        {
            var person = persons.First(c => c.Name == name);
            return Task.FromResult(person);
        }
        
    }
}
