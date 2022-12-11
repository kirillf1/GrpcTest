using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public class RoleDto
    {
        public string Name { get; set; } = default!;
        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
