using Core.Contracts;
using Core.Permissions;
using Core.Persons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UI
{
    public class PersonsViewModel : ReactiveObject
    {
        private readonly IPersonService personService;
        private readonly IPersonContext personContext;

        public PersonsViewModel(IPersonService personService, IPersonContext personContext)
        {
            this.personService=personService;
            this.personContext=personContext;
            Init();

        }
        private async Task Init()
        {
            var persons = await personService.GetAllPersons();
            Persons = new(persons);
            CanEdit = await personContext.HasPermitted(PermissionTypes.CanEdit);
            
        }
        [Reactive] public bool CanEdit { get; set; } = false;
        [Reactive] public ObservableCollection<PersonDto> Persons { get; set; } = new ObservableCollection<PersonDto>();
    }
}
