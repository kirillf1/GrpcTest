using Core.Permissions;
using Core.Persons;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Roles
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
            var enitityRep = new TestEntityRepositoryInMemory();
            var entities = enitityRep.GetEnities().Result;
            var editPermission = new Permission(1, PermissionTypes.CanEdit);
            var canWatchSomth = new Permission(2, PermissionTypes.CanWatch);
            var adminRole = new Role(1, "Admin", new List<Permission> { canWatchSomth, editPermission }, Enumerable.Empty<PermissionData>());
            var userRole = new Role(2, "User", new List<Permission> { canWatchSomth }, Enumerable.Empty<PermissionData>());
            var unknownRole = new Role(3, "Unknown", new List<Permission>(), Enumerable.Empty<PermissionData>());
            adminRole.AddDataPermission(new PermissionData(PermissionDataTypes.Entity, entities.Where(c => c.Content.Contains("Admin"))
                .Select(c => c.Id)));
            userRole.AddDataPermission(new PermissionData(PermissionDataTypes.Entity, entities.Where(c => c.Content.Contains("User"))
               .Select(c => c.Id)));
            Roles.Add(adminRole);
            Roles.Add(userRole);
            Roles.Add(unknownRole);
        }

        public Task<List<Role>> GetRole()
        {
            return Task.FromResult(Roles);
        }

        public Task<Role> CreateRole(string name, IEnumerable<Permission> permisions)
        {
            var role = new Role(Roles.Count + 1, name, permisions, Enumerable.Empty<PermissionData>());
            Roles.Add(role);
            return Task.FromResult(role);
        }

        public Task<Role> GetRoleByName(string roleName)
        {
            return Task.FromResult(Roles.First(c => c.Name == roleName));
        }
    }
}
