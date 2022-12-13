using Core.Contracts;
using Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Persons
{
    public class SecurityPersonServiceDecorator : IPersonService
    {
        private readonly IPersonService personService;
        private readonly IPersonContext personContext;

        public SecurityPersonServiceDecorator(IPersonService personService, IPersonContext personContext)
        {
            this.personService = personService;
            this.personContext = personContext;
        }

        public async Task AddPerson(string personName, string password, string? roleName = null)
        {
            var canDelete = await personContext.HasPermitted(PermissionTypes.CanEdit);
            if (!canDelete)
                // Просто показано для примера, можно возращать просто, можно изменить интерфейс IPersonService
                // чтобы пользователю видно, что нет доступа 
                throw new Exception("Пользователь не имеет прав");
            await personService.AddPerson(personName, password, roleName);
        }

        public async Task DeletePerson(string name)
        {
            var canDelete = await personContext.HasPermitted(PermissionTypes.CanEdit);
            if (!canDelete)
                // Просто показано для примера, можно возращать просто, можно изменить интерфейс IPersonService
                // чтобы пользователю видно, что нет доступа 
                throw new Exception("Пользователь не имеет прав");
            await personService.DeletePerson(name);
        }

        public async Task<List<PersonDto>> GetAllPersons()
        {
            var canWatch = await personContext.HasPermitted(PermissionTypes.CanWatch);
            if (!canWatch)
                throw new Exception("Нет доступа!");
            return await personService.GetAllPersons();
        }

        public async Task<PersonWithPasswordDto> GetPersonWithPassword(string personName)
        {
            var person = await personContext.GetPersonInfo();
            if (person is null)
                throw new Exception("Нет данных");
            if (person.Name != personName)
                throw new Exception("Нет доступа к пользователю");
            return await personService.GetPersonWithPassword(personName);
        }
    }
}
