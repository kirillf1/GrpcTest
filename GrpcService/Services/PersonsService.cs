using Core.Repositories;
using Core.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services
{
    public class PersonsService : PersonService.PersonServiceBase
    {
        private readonly IPersonRepository personRepository;
        private readonly IRoleRepository roleRepository;

        public PersonsService(IPersonRepository personRepository, IRoleRepository roleRepository)
        {
            this.personRepository = personRepository;
            this.roleRepository = roleRepository;
        }
        public async override Task<Empty> DeletePerson(PersonName request, ServerCallContext context)
        {
            var personContext = GetPersonContextFromHeaders(context);
            if (personContext is null || !await personContext.HasPermitted(Core.Persons.PermissionTypes.CanEdit))
                throw new Exception("Cant delete");
            var persons = await personRepository.GetAllPersons();
            var personToDelete = persons.FirstOrDefault(c => c.Name == request.Name);
            if (personToDelete is not null)
                persons.Remove(personToDelete);
            return new Empty();
        }
        public async override Task GetAllPersons(Empty request, IServerStreamWriter<Person> responseStream, ServerCallContext context)
        {
            var personContext = GetPersonContextFromHeaders(context);
            if (personContext is null || !await personContext.HasPermitted(Core.Persons.PermissionTypes.CanWatch))
                return;
            var persons = await personRepository.GetAllPersons();
            foreach (var person in persons)
            {
                await responseStream.WriteAsync(new Person { Name =  person.Name, RoleName = person.Role?.Name ?? "" });
            }
        }
        public async override Task<PersonWithPassword> GetPersonWithPassword(PersonName request, ServerCallContext context)
        {
            var personContext = GetPersonContextFromHeaders(context);
            if (personContext is null)
                throw new Exception();
            var caller = await personContext.GetPersonInfo();
            if (caller is null || !caller.Name.Equals(request.Name))
                throw new Exception();
            var person = await personRepository.GetPersonByName(request.Name);
            return new PersonWithPassword() { Name = person.Name, Password = person.Password, RoleName = person.Role?.Name ?? "" };
        }
        public override async Task<Person> AddPerson(PersonCreate request, ServerCallContext context)
        {
            var personContext = GetPersonContextFromHeaders(context);
            if (personContext is null || !await personContext.HasPermitted(Core.Persons.PermissionTypes.CanEdit))
                throw new Exception();
            int? roleId = null;
            if (request.HasRoleName)
            {
                var role = await roleRepository.GetRoleByName(request.Name);
                roleId = role.Id;
            }
            var person = await personRepository.CreatePerson(request.Name, request.Password, roleId);
            //grpc
            return new Person { Name =  person.Name, RoleName = person.Role?.Name ?? "" };
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
