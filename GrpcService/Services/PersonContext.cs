using Core.Contracts;
using Core.Permissions;
using Core.Persons;
using Core.Repositories;

namespace GrpcService.Services
{
    public class PersonContext : IPersonContext
    {
        private readonly Person person;

        public PersonContext(Core.Persons.Person person)
        {
            this.person = person;
        }

        public Task<PersonDto?> GetPersonInfo()
        {
            return Task.FromResult(new PersonDto { Name = person.Name, RoleName = person.Role?.Name });
        }

        public Task<RoleDto?> GetPersonRole()
        {
            if (person.Role is null)
                return default;
            var role = new RoleDto { Name = person.Role.Name, Permissions = new List<Permission>(person.Role.Permissions)};
            return Task.FromResult<RoleDto>(role);
        }

        public Task<bool> HasPermitted(string permissionDesctription)
        {
            if (person.Role is null)
                return Task.FromResult(false);
            return Task.FromResult(person.Role.HasPermission(permissionDesctription));
        }
    }
}
