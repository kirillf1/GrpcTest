using Core.Contracts;
using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IRoleService
    {
        public Task<bool> HasPermitted(string roleName, string permission);
        public Task<List<RoleDto>> GetAllRoles();
        public Task CreateRole(RoleDto roleDto);
        public Task DeleteRole(string roleName);
        public Task UpdatePermissions(string roleName, IEnumerable<Permission> permissions);
    }
}
