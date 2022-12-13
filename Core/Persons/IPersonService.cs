using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    public interface IPersonService
    {
        public Task<PersonWithPasswordDto> GetPersonWithPassword(string personName);
        public Task<List<PersonDto>> GetAllPersons();
        public Task DeletePerson(string name);
        public Task AddPerson(string personName, string password, string? roleName = null);
    }
}
