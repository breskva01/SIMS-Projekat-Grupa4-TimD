using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Storage;
using InitialProject.Application.Stores;
using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly UserService _userService;
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
            new NavigateCommand(new NavigationService(_navigationStore, CreateAccommodationBrowserViewModel()));
        public ICommand GuideNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, CreateGuideVM()));
        //public ICommand Guest1NavigateCommand { get; }

        public ICommand OwnerNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, GuestRatingVM()));
        private readonly NavigationStore _navigationStore;
        private User _user;
        public SignInViewModel(NavigationStore navigationStore)
        {

            _userService = new UserService();       
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
        private AccommodationBrowserViewModel CreateAccommodationBrowserViewModel()
        {
            return new AccommodationBrowserViewModel(_navigationStore, _user);
        }
        private TourBrowserViewModel CreateGuest2VM()
        {
            return new TourBrowserViewModel(_navigationStore, _user);
        }
        /*
        private TourCreationViewModel CreateGuideVM()
        {
            return new TourCreationViewModel(_navigationStore, _user);
        }
        */
        private ToursTodayViewModel CreateGuideVM()
        {
            return new ToursTodayViewModel(_navigationStore, _user);
        }
        /*
        private AllToursViewModel CreateGuideVM()
        {
            return new AllToursViewModel(_navigationStore, _user);
        }*/


        private AccommodationRegistrationViewModel CreateOwnerVM()
        {
            return new AccommodationRegistrationViewModel(_navigationStore, _user);
        }
        private GuestRatingViewModel GuestRatingVM()
        {
            return new GuestRatingViewModel(_navigationStore, _user);
        }
    }
}
