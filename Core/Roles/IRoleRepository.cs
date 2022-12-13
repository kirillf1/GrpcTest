using Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Roles
{
    public interface IRoleRepository
    {
        public Task<Role> GetRoleByName(string roleName);
        public Task<List<Role>> GetRole();
        public Task<Role> CreateRole(string name, IEnumerable<Permission> permisions);

    }
}
