using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persons
{
    public class PermissionData
    {
        public PermissionData(string name, IEnumerable<int> ids)
        {
            Name = name;
            this.ids = new List<int>(ids);
        }
        private List<int> ids;
        public string Name { get; set; }
        public IReadOnlyCollection<int> DataIds { get => ids; }
    }
}
