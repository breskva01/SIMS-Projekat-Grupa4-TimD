using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using InitialProject.WPF.ViewModels.GuestOne;
using InitialProject.WPF.ViewModels.GuestTwo;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace InitialProject.WPF.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;

        private readonly AccommodationReservationService _reservationService;
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationService _accommodationService;
        private List<Accommodation> _accommodations;
        private readonly AccommodationRenovationService _renovationService;
        private List<AccommodationRenovation> _renovations;
        private readonly UserNotificationService _userNotificationService;

        public SecureString Password { get; set; }
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SignInCommand { get; }
        public ICommand Guest2NavigateCommand => 
            new NavigateCommand(new NavigationService(_navigationStore, CreateGuest2VM()));
        public ICommand Guest1NavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, NavigateAccommodationBrowser()));
        public ICommand GuideNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, CreateGuideVM()));

        public ICommand OwnerNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, OwnerMainMenuVM()));
        private readonly NavigationStore _navigationStore;
        private User _user;
        public SignInViewModel(NavigationStore navigationStore)
        {

            _userService = new UserService();       
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            SignInCommand = new SignInCommand(SignIn);
            _reservationService = new AccommodationReservationService();
            _accommodationService = new AccommodationService();
            _renovationService = new AccommodationRenovationService();
            _userNotificationService = new UserNotificationService();
            _navigationStore = navigationStore;
        }

        private void SignIn(string password)
        {
            _user = _userService.GetByUsername(Username);
            if (_user != null)
            {
                if (_user.Password.Equals(password))
                {
                    OpenAppropriateWindow(_user);
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }
        private void OpenAppropriateWindow(User user)
        {
            if (user is Owner owner)
            {
                OwnerNavigateCommand.Execute(null);
            }
            else if(user is Guest1)
                Guest1NavigateCommand.Execute(null);
            else if(user is TourGuide)
                GuideNavigateCommand.Execute(null);
            else
                Guest2NavigateCommand.Execute(null);
        }
        private ViewModelBase NavigateAccommodationBrowser()
        {
            var accommodationBrowserViewModel = 
                new AccommodationBrowserViewModel(_navigationStore, (Guest1)_user);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, (Guest1)_user);
            return new LayoutViewModel(navigationBarViewModel, accommodationBrowserViewModel);
        }
        private ViewModelBase CreateGuest2VM()
        {
            if (_tourReservationService.GetActivePending(_user.Id).Any())
            {
                return new PresenceConfirmationViewModel(_navigationStore, _user);
            }
            return new Guest2MenuViewModel(_navigationStore, _user);
        }

        private GuideMenuViewModel CreateGuideVM()
        {
            return new GuideMenuViewModel(_navigationStore, _user);
        }

        private OwnerMainMenuViewModel OwnerMainMenuVM()
        {
            _user = _userService.GetByUsername(Username);
            bool IsNotified = true;
            _reservations = _reservationService.FindCompletedAndUnrated(_user.Id);
            List<UserNotification> notifications = _userNotificationService.GetByUser(_user.Id);
            foreach (AccommodationReservation res in _reservations)
            {
                if (DateOnly.FromDateTime(DateTime.Now) > res.LastNotification)
                {
                    _reservationService.UpdateLastNotification(res);
                    _userNotificationService.CreateNotification(_user.Id, "You have unrated guests!", DateTime.Now);
                    IsNotified = false;
                }
            }
            foreach(UserNotification userNotification in notifications)
            {
                if(!userNotification.IsRead) 
                {
                    IsNotified = false;
                    break;
                }
            }
            _accommodations = _accommodationService.GetAllOwnersAccommodations(_user.Id);
            _renovations = _renovationService.GetAll();
            foreach(Accommodation accommodation in _accommodations)
            {
                foreach(AccommodationRenovation renovation in _renovations)
                {
                    if(renovation.Accommodation.Id == accommodation.Id)
                    {
                        if (DateTime.Now >= renovation.RenovationExpiration)
                        {
                            _accommodationService.UpdateRenovationStatus(accommodation.Id);
                            _renovationService.UpdateStatus(renovation.Id);
                        }
                        else if (DateTime.Now >= renovation.End)
                        {
                            _accommodationService.UpdateRenovationStatus(accommodation.Id);
                            _renovationService.UpdateStatus(renovation.Id);
                        }
                        
                    }
                }
            }
            return new OwnerMainMenuViewModel(_navigationStore, (Owner)_user, IsNotified);
        }
    }
}
