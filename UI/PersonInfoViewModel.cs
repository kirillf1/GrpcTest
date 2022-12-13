using Core.Persons;
using Core.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public class PersonInfoViewModel : ReactiveObject
    {
        private readonly IPersonService personService;
        private readonly IPersonContext personContext;

        public PersonInfoViewModel(IPersonService personService, IPersonContext personContext)
        {
            this.personService=personService;
            this.personContext=personContext;
            Init();

        }
        private async Task Init()
        {
            var canWatch = await personContext.HasPermitted(PermissionTypes.CanWatch);
            if (!canWatch)
                return;
            var person = await personContext.GetPersonInfo();
            var personInfo = await personService.GetPersonWithPassword(person.Name);
            Name = personInfo.Name;
            Password = personInfo.Password;

        }
        [Reactive] public string Name { get; set; } = "";
        [Reactive] public string Password { get; set; } = "";

    }
}
