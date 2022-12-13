using Core.Entities;
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
    public class EnititesViewModel : ReactiveObject
    {
        private readonly ITestEntityService testEntityService;

        public EnititesViewModel(ITestEntityService testEntityService)
        {
            
            this.testEntityService=testEntityService;
            Init();
        }
        private async Task Init()
        {
            Enities = new ObservableCollection<TestEnity>(await testEntityService.GetEnities());
        }
        
        [Reactive] public ObservableCollection<TestEnity> Enities { get; set; } = new ObservableCollection<TestEnity>();
    }
}
