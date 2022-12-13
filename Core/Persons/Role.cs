using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    public class Role
    {
        public Role(int id,string name, IEnumerable<Permission> permissions, IEnumerable<PermissionData> dataPermissions)
        {
            Id=id;
            Name = name;
            this.permissions = new List<Permission>(permissions);
            this.dataPermissions = new List<PermissionData>(dataPermissions);
        }

        public int Id { get; }
        public string Name { get; set; }
        private List<Permission> permissions;
        private List<PermissionData> dataPermissions;
        public IReadOnlyCollection<PermissionData> DataPermissions { get => dataPermissions; }
        public IReadOnlyCollection<Permission> Permissions { get => permissions; }
        public void AddPermission(Permission permision)
        {
            permissions.Add(permision);
        }
        // это проблемный аспект, так как мы обращаемся по строке подумать, как это поменять
        public bool HasPermission(string description)
        {
            return permissions.Any(p => p.Desctription.Equals(description,StringComparison.OrdinalIgnoreCase));
        }
        public void AddDataPermission(PermissionData permision)
        {
            dataPermissions.Add(permision);
        }
        public IEnumerable<int> GetAvailableData(string permissionName)
        {
            var dataPermission = dataPermissions.FirstOrDefault(c => c.Name == permissionName);
      
            return dataPermission?.DataIds ?? Enumerable.Empty<int>();
        }
    }
}
