using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Guest1 _user;
        private bool _anyNotifications;
        public bool AnyNotifications
        {
            get => _anyNotifications;
            set
            {
                if (_anyNotifications != value)
                {
                    _anyNotifications = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedTab;
        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NavigateAccommodationBrowserCommand { get; }
        public ICommand NavigateAnywhereAnytimeCommand { get; }
        public ICommand NavigateMyResevationsCommand { get; }
        public ICommand NavigateMyRequestsCommand { get; }
        public ICommand NavigateRatingsCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand OpenNotificationsPromptCommand { get; }
        public NavigationBarViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _navigationStore = navigationStore;
            _user = user;
            CheckForNotifications();
            NavigateAccommodationBrowserCommand = new ExecuteMethodCommand(NavigateAccommodationBrowser);
            NavigateAnywhereAnytimeCommand = new ExecuteMethodCommand(NavigateAnywhereAnytimeBrowser);
            NavigateMyResevationsCommand = new ExecuteMethodCommand(NavigateMyReservations);
            NavigateMyRequestsCommand = new ExecuteMethodCommand(NavigateMyRequests);
            NavigateRatingsCommand = new ExecuteMethodCommand(NavigateRatings);
            NavigateLoginCommand = new ExecuteMethodCommand(NavigateLogin);
            OpenNotificationsPromptCommand = new ExecuteMethodCommand(OpenNotificationsPrompt);
        }

        private void CheckForNotifications()
        {
            var requestService = new AccommodationReservationRequestService();
            if (requestService.GetAllNewlyAnswered(_user.Id).Count > 0)
            {
                AnyNotifications = true;
            }
            else
                AnyNotifications = false;
        }
        private void OpenNotificationsPrompt()
        {
            var requestService = new AccommodationReservationRequestService();
            requestService.UpdateGuestNotifiedField(_user.Id);
            MessageBoxResult result = MessageBox.Show(
                   "Stiglo je jedan ili više novih odgovora na vaše zahteve," +
                   "da li želite da ih pogledate?",
                   "Obaveštenje", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                NavigateMyRequests();
            AnyNotifications = false;
        }
        private void NavigateAccommodationBrowser()
        {
            ViewModelBase viewModel = new AccommodationBrowserViewModel(_navigationStore, _user);
            viewModel = CreateLayoutViewModel(viewModel);
            Navigate(viewModel);
        }
        private void NavigateAnywhereAnytimeBrowser()
        {
            ViewModelBase viewModel = new AnywhereAnytimeViewModel(_navigationStore, _user);
            viewModel = CreateLayoutViewModel(viewModel);
            Navigate(viewModel);
        }
        private void NavigateMyReservations()
        {
            ViewModelBase viewModel = new MyAccommodationReservationsViewModel(_navigationStore, _user);
            viewModel = CreateLayoutViewModel(viewModel);
            Navigate(viewModel);
        }
        private void NavigateMyRequests()
        {
            ViewModelBase viewModel = new MyAccommodationReservationRequestsViewModel(_navigationStore, _user);
            viewModel = CreateLayoutViewModel(viewModel);
            Navigate(viewModel);
        }
        private void NavigateRatings()
        {
            ViewModelBase viewModel = new AccommodationRatingViewModel(_navigationStore, _user);
            viewModel = CreateLayoutViewModel(viewModel);
            Navigate(viewModel);
        }   
        private void NavigateLogin()
        {
            var viewModel = new SignInViewModel(_navigationStore);
            Navigate(viewModel);
        }
        private ViewModelBase CreateLayoutViewModel(ViewModelBase contentViewModel)
        {
            var navigateBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigateBarViewModel, contentViewModel);
            return layoutViewModel;
        }
        private void Navigate(ViewModelBase viewModel)
        {
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }
}