using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public interface ITestEntityService
    {
        public Task<List<TestEnity>> GetEnities();
    }
}
