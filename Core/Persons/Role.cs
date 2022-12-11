using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    public class Role
    {
        public Role(int id,string name, IEnumerable<Permission> permissions)
        {
            Id=id;
            Name = name;
            this.permissions = new List<Permission>(permissions);
        }

        public int Id { get; }
        public string Name { get; set; }
        private List<Permission> permissions;
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
    }
}
