using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Permissions
{
    public interface IPermissionService
    {
        public Task<List<Permission>> GetPermissions();
    }
}
