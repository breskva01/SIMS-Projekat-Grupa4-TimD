using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
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
        public ICommand NavigateForumsCommand { get; }
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
            NavigateForumsCommand = new ExecuteMethodCommand(NavigateForums);
            NavigateLoginCommand = new ExecuteMethodCommand(NavigateSignIn);
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
            string messageBoxText = "";
            string messageBoxCaption = "";
            if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
            {
                messageBoxText = "Stiglo je jedan ili više novih odgovora na vaše zahteve, da li želite da ih pogledate?";
                messageBoxCaption = "Obaveštenje";
            }
            else
            {
                messageBoxText = "One or more new responses for your requests have arrived. Would you like to take a look?";
                messageBoxCaption = "Notification";
            }
            MessageBoxResult result = MessageBox.Show(messageBoxText, messageBoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                NavigateMyRequests();
            AnyNotifications = false;
        }
        private void NavigateAccommodationBrowser()
        {
            Navigate(ViewModelFactory.Instance.CreateAccommodationBrowserVM(_navigationStore, _user));
        }
        private void NavigateAnywhereAnytimeBrowser()
        {
            Navigate(ViewModelFactory.Instance.CreateAnywhereAnytimeVM(_navigationStore, _user));
        }
        private void NavigateMyReservations()
        {
            Navigate(ViewModelFactory.Instance.CreateMyReservationsVM(_navigationStore, _user));
        }
        private void NavigateMyRequests()
        {
            Navigate(ViewModelFactory.Instance.CreateMyRequestsVM(_navigationStore, _user));
        }
        private void NavigateRatings()
        {
            Navigate(ViewModelFactory.Instance.CreateRatingsVM(_navigationStore, _user));
        }
        private void NavigateForums()
        {
            Navigate(ViewModelFactory.Instance.CreateForumBrowserVM(_navigationStore, _user));
        }
        private void NavigateSignIn()
        {
            Navigate(ViewModelFactory.Instance.CreateSignInVM(_navigationStore));
        }
        private void Navigate(ViewModelBase viewModel)
        {
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}