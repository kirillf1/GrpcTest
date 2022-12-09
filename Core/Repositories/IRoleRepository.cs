using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetRole();
        public Task<Role> CreateRole(string name,IEnumerable<Permision> permisions);
        
    }
}
