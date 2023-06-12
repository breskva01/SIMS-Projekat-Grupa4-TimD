using InitialProject.Application.Factories;
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
        public static NavigationService Instance { get; set; }
        public NavigationService(NavigationStore navigationStore, ViewModelBase viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }
        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _viewModel;
        }
        public void Navigate(ViewModelBase viewModel)
        {
            _navigationStore.CurrentViewModel = viewModel;
        }
    }
}
