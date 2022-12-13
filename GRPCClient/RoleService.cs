using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcService;
using Grpc.Core;
using Core.Permissions;
using Core.Roles;

namespace GRPCClient
{
    public class RoleService : IRoleService
    {
        private readonly GrpcService.RoleService.RoleServiceClient client;
        // пока без авторизации, так как роли все могут посмотреть
        public RoleService(GrpcService.RoleService.RoleServiceClient client)
        {
            this.client=client;
        }
        public async Task<List<RoleDto>> GetAllRoles()
        {
            var rolesGrpc =  client.GetAllRoles(new Google.Protobuf.WellKnownTypes.Empty());
            var roles = new List<RoleDto>();
            await foreach (var item in rolesGrpc.ResponseStream.ReadAllAsync())
            {
                roles.Add(new RoleDto { Name = item.Name, Permissions = item.Permissions
                    .Select(c => new Permission(c.Id, c.Description)).ToList() });
            }
            return roles;
        }
        public async Task<bool> HasPermitted(string roleName, string permission)
        {
            var isAllowed = await client.HasPermittedAsync(new RoleNameWithPermission { Permission = permission, RoleName = roleName });
            return isAllowed.IsAllowed;
        }
        // остальное пока лень делать, если нужно будет редактировать роли то закончу
        public Task CreateRole(RoleDto roleDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRole(string roleName)
        {
            throw new NotImplementedException();
        }



        public Task UpdatePermissions(string roleName, IEnumerable<Permission> permissions)
        {
            throw new NotImplementedException();
        }
    }
}
