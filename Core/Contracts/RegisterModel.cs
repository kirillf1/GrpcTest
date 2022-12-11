using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public class RegisterModel
    {
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Role { get; set; } = default!;
    }
}
