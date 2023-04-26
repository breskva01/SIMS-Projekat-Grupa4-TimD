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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly User _user;

        public ICommand NavigateAccommodationBrowserCommand { get; }
        public ICommand NavigateMyResevationsCommand { get; }
        public ICommand NavigateMyRequestsCommand { get; }
        public ICommand NavigateRatingsCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public NavigationBarViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            NavigateAccommodationBrowserCommand = new ExecuteMethodCommand(NavigateAccommodationBrowser);
            NavigateMyResevationsCommand = new ExecuteMethodCommand(NavigateMyReservations);
            NavigateMyRequestsCommand = new ExecuteMethodCommand(NavigateMyRequests);
            NavigateRatingsCommand = new ExecuteMethodCommand(NavigateRatings);
            NavigateLoginCommand = new ExecuteMethodCommand(NavigateLogin);
        }
        private void NavigateAccommodationBrowser()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, _user);
            CreateLayoutViewModel(viewModel);
        }
        private void NavigateMyReservations()
        {
            var viewModel = new MyAccommodationReservationsViewModel(_navigationStore, _user);
            CreateLayoutViewModel(viewModel);
        }
        private void NavigateMyRequests()
        {
            var viewModel = new MyAccommodationReservationRequestsViewModel(_navigationStore, _user);
            CreateLayoutViewModel(viewModel);
        }
        private void NavigateRatings()
        {
            var viewModel = new AccommodationRatingViewModel(_navigationStore, _user);
            CreateLayoutViewModel(viewModel);
        }   
        private void NavigateLogin()
        {
            var viewModel = new SignInViewModel(_navigationStore);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
        private void CreateLayoutViewModel(ViewModelBase contentViewModel)
        {
            var navigateBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigateBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
    }
}
