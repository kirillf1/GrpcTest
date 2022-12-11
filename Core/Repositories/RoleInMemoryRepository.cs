using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class RoleInMemoryRepository : IRoleRepository
    {
        static RoleInMemoryRepository()
        {
            Seed();
        }
        static List<Role> Roles = new List<Role>();
      
        private static void Seed()
        {
            var editPermission = new Permission(1, PermissionTypes.CanEdit);
            var canWatchSomth = new Permission(2, PermissionTypes.CanWatch);
            Roles.Add(new Role(1, "Admin", new List<Permission> { canWatchSomth, editPermission }));
            Roles.Add(new Role(2, "User", new List<Permission> { canWatchSomth }));
        }

        public Task<List<Role>> GetRole()
        {
            return Task.FromResult(Roles);
        }

        public Task<Role> CreateRole(string name, IEnumerable<Permission> permisions)
        {
            var role = new Role(Roles.Count +1,name,permisions);
            Roles.Add(role);
            return Task.FromResult(role);
        }
    }
}
