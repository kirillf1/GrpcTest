using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public class PersonDto
    {
        public string Name { get; set; } = default!;
        public string? RoleName { get; set; }
    }
}
