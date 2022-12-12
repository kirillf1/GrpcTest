using Core.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static GrpcService.RoleService;

namespace GrpcService.Services
{
    public class RolesService : RoleServiceBase
    {
        private readonly IRoleRepository rolesRepository;

        public RolesService(IRoleRepository rolesRepository)
        {
            this.rolesRepository=rolesRepository;
        }
        public override async Task GetAllRoles(Empty request, IServerStreamWriter<RoleGrpc> responseStream, ServerCallContext context)
        {
            var roles = await rolesRepository.GetRole();
            foreach (var role in roles)
            {
                var roleGrpc = new RoleGrpc
                {
                    Name = role.Name,
                };
                 
               roleGrpc.Permissions.AddRange(role.Permissions.Select(c => new PermissionGrpc
                {
                    Description = c.Desctription,
                    Id = c.Id
                }));
                await responseStream.WriteAsync(roleGrpc);
            }
        }
        public override async Task<IsPermitted> HasPermitted(RoleNameWithPermission request, ServerCallContext context)
        {
            try
            {
                var role = await rolesRepository.GetRoleByName(request.RoleName);
                var isAllowed = role.HasPermission(request.Permission);
                return new IsPermitted { IsAllowed = isAllowed };
            }
            catch
            {
                return new IsPermitted { IsAllowed = false };
            }
        }
    }
}
