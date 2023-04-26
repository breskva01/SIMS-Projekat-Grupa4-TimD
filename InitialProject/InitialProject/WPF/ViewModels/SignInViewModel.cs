using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using InitialProject.WPF.ViewModels.Guest1;
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
        //public ICommand Guest1NavigateCommand { get; }

        public ICommand OwnerNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, OwnerVM()));
        private readonly NavigationStore _navigationStore;
        private User _user;
        public SignInViewModel(NavigationStore navigationStore)
        {

            _userService = new UserService();       
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            SignInCommand = new SignInCommand(SignIn);
            _reservationService = new AccommodationReservationService();
            _navigationStore = navigationStore;
        }

        private void SignIn(String password)
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
            switch (user.Type)
            {
                case UserType.Owner:
                    {
                        int UnratedGuests = 0;
                        bool IsNotified = true;
                        _reservations = _reservationService.FindCompletedAndUnrated(user.Id);
                        foreach (AccommodationReservation res in _reservations)
                        {
                            if (DateOnly.FromDateTime(DateTime.Now) > res.LastNotification)
                            {
                                _reservationService.updateLastNotification(res);
                                UnratedGuests++;
                                IsNotified= false;
                            }

                        }
                        if (UnratedGuests > 0 && !IsNotified)
                            MessageBox.Show("You have " + UnratedGuests.ToString() + " unrated guests!");

                        OwnerNavigateCommand.Execute(null);
                        break;
                    }
                case UserType.Guest1:
                    {
                        Guest1NavigateCommand.Execute(null);
                        break;
                    }
                case UserType.TourGuide:
                    {
                        GuideNavigateCommand.Execute(null);
                        break;
                    }
                case UserType.Guest2:
                    {
                        Guest2NavigateCommand.Execute(null);
                        break;
                    }
            }
        }
        private ViewModelBase NavigateAccommodationBrowser()
        {
            var accommodationBrowserViewModel = new AccommodationBrowserViewModel(_navigationStore, _user);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
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

        private OwnerViewModel OwnerVM()
        {
            return new OwnerViewModel(_navigationStore, _user);
        }
    }
}
