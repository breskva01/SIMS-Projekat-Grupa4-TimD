using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class Guest2RequestMenuViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;

        public ICommand TourRequestCommand { get; }
        public ICommand ComplexTourRequestCommand { get; }
        public ICommand RequestStatsCommand { get; }
        public ICommand MenuCommand { get; }
        public Guest2RequestMenuViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            
            TourRequestCommand = new ExecuteMethodCommand(ShowRequestView);
            ComplexTourRequestCommand = new ExecuteMethodCommand(ShowComplexRequestView);
            RequestStatsCommand = new ExecuteMethodCommand(ShowRequestStatsView);
            MenuCommand = new ExecuteMethodCommand(ShowMainMenuView);
        }

        
        private void ShowRequestView()
        {
            TourRequestViewModel tourRequestViewModel = new TourRequestViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestViewModel));

            navigate.Execute(null);
        }
        
        
        private void ShowComplexRequestView()
        {
            ComplexTourRequest1ViewModel complexTourRequest1ViewModel = new ComplexTourRequest1ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, complexTourRequest1ViewModel));

            navigate.Execute(null);
        }
        
        private void ShowRequestStatsView()
        {
            TourRequestStats1ViewModel tourRequestStats1ViewModel = new TourRequestStats1ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStats1ViewModel));

            navigate.Execute(null);
        }
        

        private void ShowMainMenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));

            navigate.Execute(null);
        }

    }
    
}
