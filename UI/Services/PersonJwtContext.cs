using Core.Contracts;
using Core.Persons;
using Core.Roles;
using Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Services
{
    public class PersonJwtContext : IPersonContext
    {
        private readonly IRoleService roleService;
        private readonly ITokenProvider tokenProvider;

        public PersonJwtContext(IRoleService roleService, ITokenProvider tokenProvider)
        {
            this.roleService = roleService;
            this.tokenProvider = tokenProvider;
        }
        public async Task<PersonDto?> GetPersonInfo()
        {
            var token = await tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var personName = jwtSecurityToken.Claims.First(c => c.Type == "Name").Value;
            var roleName = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value ?? "";
            return new PersonDto { Name = personName, RoleName = roleName };
        }

        public async Task<RoleDto?> GetPersonRole()
        {
            var token = await tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var roleName = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value ?? "";
            return new RoleDto { Name = roleName };
        }

        public async Task<bool> HasPermitted(string permissionDesctription)
        {
            var token = await tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var roleName = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value ?? "";
            return await roleService.HasPermitted(roleName, permissionDesctription);
        }
    }
}
