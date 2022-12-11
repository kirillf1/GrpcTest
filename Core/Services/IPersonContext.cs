using Core.Contracts;
using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPersonContext
    {
        public Task<RoleDto?> GetPersonRole();
        public Task<PersonDto?> GetPersonInfo();
        public Task<bool> HasPermitted(string permissionDesctription);
    }
}
