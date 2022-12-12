using Core.Contracts;
using Core.Services;
using Grpc.Core;
using GrpcService.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRPCClient
{
    public class PersonServiceGrpc : IPersonService
    {
        private readonly PersonService.PersonServiceClient personService;
        private readonly ITokenProvider tokenProvider;

        public PersonServiceGrpc(PersonService.PersonServiceClient personService, ITokenProvider tokenProvider)
        {
            this.personService=personService;
            this.tokenProvider=tokenProvider;
        }
        public async Task AddPerson(string personName, string password, string? roleName = null)
        {
            var metadata = await GetAuthMetadata();
            await personService.AddPersonAsync(new PersonCreate
            {
                Name = personName,
                Password = password,
                RoleName = roleName ?? string.Empty,
            }, metadata);
        }

        public async Task DeletePerson(string name)
        {
            var metadata = await GetAuthMetadata();
            await personService.DeletePersonAsync(new PersonName
            {
                Name = name
            }, metadata);
        }

        public async Task<List<PersonDto>> GetAllPersons()
        {
            var metadata = await GetAuthMetadata();
            var personsAsync = personService.GetAllPersons(new Google.Protobuf.WellKnownTypes.Empty(),metadata);
            var persons = new List<PersonDto>();
            await foreach (var person in personsAsync.ResponseStream.ReadAllAsync())
            {
                persons.Add(new PersonDto { Name = person.Name, RoleName = person.RoleName });
            }
            return persons;
        }

        public async Task<PersonWithPasswordDto> GetPersonWithPassword(string personName)
        {
            var metadata = await GetAuthMetadata();
            var person = await personService.GetPersonWithPasswordAsync(new PersonName { Name = personName }, metadata);
            return new PersonWithPasswordDto { Name = person.Name, RoleName = person.RoleName, Password = person.Password };
        }
        // можно было вынести в декоратор отдельный добавления авторизации, но мне облом, для примера пойдет
        private async Task<Metadata> GetAuthMetadata()
        {
            var token = await tokenProvider.GetTokenAsync();
            return new Metadata() { { "Authorization", token } };
        }
    }
}
