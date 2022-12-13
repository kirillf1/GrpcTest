using Core.Contracts;
using Core.Persons;
using Core.Roles;
using Core.Services;
using Grpc.Core;
using GrpcService.Protos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Person = GrpcService.Protos.Person;

namespace GrpcService.Services
{
    public class AuthService : Auth.AuthBase
    {
        private readonly IPersonRepository personRepository;
        private readonly IRoleRepository roleRepository;

        public AuthService(IPersonRepository personRepository, IRoleRepository roleRepository)
        {
            this.personRepository = personRepository;
            this.roleRepository = roleRepository;
        }
        public override async Task<Person> Login(LoginRequest request, ServerCallContext context)
        {
            Core.Persons.Person person = await personRepository.GetPersonByName(name: request.Name);
            if (person == null || !request.Password.Equals(person.Password))
                return default;

            var secret = "secret secret secret secret secret";
            await context.WriteResponseHeadersAsync(new Metadata() { { "Authorization", GenerateToken(secret, person) } });
            return new Person { Name = person.Name, RoleName = person.Role?.Name ?? "" };
        }
        public override async Task<Protos.Person> Register(RegisterRequest request, ServerCallContext context)
        {
            Role? role = null;
            if (request.HasRoleName)
            {
                var roles = await roleRepository.GetRole();
                role = roles.FirstOrDefault(r => r.Name == request.RoleName);
            }
            var person = await personRepository.CreatePerson(request.Name, request.Password, role?.Id);
            var secret = "secret secret secret secret secret";
            await context.WriteResponseHeadersAsync(new Metadata() { { "Authorization", GenerateToken(secret, person) } });
            return new Person { Name = person.Name, RoleName = person.Role?.Name ?? "" };
        }
        private static string GenerateToken(string secret, Core.Persons.Person person)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var descriptor = new SecurityTokenDescriptor
            {
                Audience = person.Id.ToString(),
                Claims = new Dictionary<string, object>
                {
                    { "Name", person.Name },
                    { "Role", person.Role?.Name ?? "" }
                },
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
