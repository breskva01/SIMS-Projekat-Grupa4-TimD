using InitialProject.Application.Stores;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewModelBase _viewModel;
        public NavigationService(NavigationStore navigationStore, ViewModelBase viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _viewModel;
        }
    }
}
