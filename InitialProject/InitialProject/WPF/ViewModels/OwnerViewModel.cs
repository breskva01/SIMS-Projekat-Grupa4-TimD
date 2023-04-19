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
using System.Xml.Serialization;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerViewModel: ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;

        public ICommand AccommodationRegistrationCommand { get; }
        public ICommand ViewRatingsCommand { get; }
        public ICommand MoveRequestsCommand { get; }
        public ICommand RateGuestCommand { get; }
        public ICommand SignOutCommand { get; }
        public OwnerViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            AccommodationRegistrationCommand = new ExecuteMethodCommand(ShowAccommodationRegistrationView);
            ViewRatingsCommand = new ExecuteMethodCommand(ShowOwnerRatingsView);
            MoveRequestsCommand = new ExecuteMethodCommand(ShowReservationMoveRequestsView);
            RateGuestCommand = new ExecuteMethodCommand(ShowGuestRatingView);
            SignOutCommand = new ExecuteMethodCommand(SignOut);

        }

        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }

        private void ShowAccommodationRegistrationView()
        {
            AccommodationRegistrationViewModel accommodationRegistrationViewModel = new AccommodationRegistrationViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, accommodationRegistrationViewModel));

            navigate.Execute(null);
        }

        private void ShowGuestRatingView()
        {
            GuestRatingViewModel guestRatingViewModel = new GuestRatingViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guestRatingViewModel));

            navigate.Execute(null);
        }

        private void ShowReservationMoveRequestsView()
        {
            ReservationMoveRequestsViewModel reservationMoveRequestsViewModel = new ReservationMoveRequestsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, reservationMoveRequestsViewModel));

            navigate.Execute(null);
        }

        private void ShowOwnerRatingsView()
        {
            OwnerRatingsViewModel ownerRatingsViewModel = new OwnerRatingsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, ownerRatingsViewModel));

            navigate.Execute(null);
        }
    }
}
