using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InitialProject.Application.Services;
using InitialProject.WPF.NewViews;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerProfileViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        public ICommand SignOutCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ShowRateGuestsViewCommand { get; }
        public ICommand ShowMyAccommodationsViewCommand { get; }
        public ICommand ShowMyRatingsViewCommand { get; }
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }
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
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value != _phoneNumber)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _rating;
        public string Rating
        {
            get => _rating;
            set
            {
                if (value != _rating)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isNotified;
        public bool IsNotified
        {
            get => _isNotified;
            set
            {
                if (value != _isNotified)
                {
                    _isNotified = value;
                    OnPropertyChanged(nameof(IsNotified));
                }
            }

        }
        public OwnerProfileViewModel(NavigationStore navigationStore, User user, bool isNotified)
        {
            _navigationStore = navigationStore;
            _user = user;
            IsNotified= isNotified;
            FirstName= user.FirstName;
            LastName= user.LastName;
            Username= _user.Username;
            Email= _user.Email;
            Rating = _user.Rating.ToString();
            PhoneNumber= _user.PhoneNumber;
            if(_user.SuperOwner)
            {
                Status = "Super owner";
            }
            else 
            {
                Status = "Owner";
            }

            SignOutCommand = new ExecuteMethodCommand(SignOut);
            BackCommand = new ExecuteMethodCommand(Back);
            ShowRateGuestsViewCommand = new ExecuteMethodCommand(ShowRateGuestsView);
            ShowMyAccommodationsViewCommand = new ExecuteMethodCommand(ShowMyAccommodationsView);
            ShowMyRatingsViewCommand = new ExecuteMethodCommand(ShowMyRatingsView);
        }
        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void Back()
        {
            OwnerMainMenuViewModel ownerMainMenuViewModel = new OwnerMainMenuViewModel(_navigationStore, _user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, ownerMainMenuViewModel));

            navigate.Execute(null);
        }
        private void ShowRateGuestsView()
        {
            GuestRatingViewModel guestRatingViewModel = new GuestRatingViewModel(_navigationStore, _user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guestRatingViewModel));

            navigate.Execute(null);
        }
        private void ShowMyAccommodationsView()
        {
            AccommodationsViewModel accommodationsViewModel = new AccommodationsViewModel(_navigationStore, _user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, accommodationsViewModel));

            navigate.Execute(null);
        }
        private void ShowMyRatingsView()
        {
            OwnerMyRatingsView ownerMyRatingsView = new OwnerMyRatingsView(_navigationStore, _user);
            ownerMyRatingsView.Show();
        }
    }
}
