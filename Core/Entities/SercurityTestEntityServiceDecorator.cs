using Core.Permissions;
using Core.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SercurityTestEntityServiceDecorator : ITestEntityService
    {
        private readonly ITestEntityService decoratee;
        private readonly IPersonContext personContext;

        public SercurityTestEntityServiceDecorator(ITestEntityService decoratee, IPersonContext personContext)
        {
            this.decoratee=decoratee;
            this.personContext=personContext;
        }
        public async Task<List<TestEnity>> GetEnities()
        {
            if (!await personContext.HasPermitted(PermissionTypes.CanWatch))
                throw new Exception("You can't watch data");
            return await decoratee.GetEnities();
        }
    }
}
