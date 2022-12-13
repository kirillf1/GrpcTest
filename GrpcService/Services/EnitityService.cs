using Core.Permissions;
using Core.Persons;
using Core.Repositories;
using Core.Roles;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static GrpcService.EntityService;

namespace GrpcService.Services
{
    public class EnitityService : EntityServiceBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly ITestDataRepository entityRepository;

        public EnitityService(IRoleRepository roleRepository,ITestDataRepository entityRepository)
        {
            this.roleRepository=roleRepository;
            this.entityRepository=entityRepository;
        }
        public override async Task GetEntities(Empty request, IServerStreamWriter<Entity> responseStream, ServerCallContext context)
        {
            var personContext = GetPersonContextFromHeaders(context);
            if (personContext is null)
                return;
            var role = await personContext.GetPersonRole();
            if (role is null)
                return;
            // сделал костыльно через репозиторий так как фильтрацию я делал в последнюю очередь, лень переписывать было roleDto
            var roleWithPermission = await roleRepository.GetRoleByName(role.Name);
            var ids = roleWithPermission.GetAvailableData(PermissionDataTypes.Entity);
            var enitities = await entityRepository.GetEnities();
            var filtredEntity = enitities.Where(e => ids.Contains(e.Id));
            foreach (var item in filtredEntity)
            {
               await responseStream.WriteAsync(new Entity { Id = item.Id, Data = item.Content });
            }

        }
        private static IPersonContext? GetPersonContextFromHeaders(ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Items.TryGetValue("Person", out var person);
            if (person is null)
                return default;
            return (IPersonContext)person;
        }
    }
}
