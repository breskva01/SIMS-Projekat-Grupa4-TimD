using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using InitialProject.WPF.NewViews.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class GuideProfileViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private UserService _userService;

        public List<Tour> GuideTours { get; set; }

        private List<int> _guestIds;
        private List<User> _guests;
        private List<User> _users;
        private List<TourReservation> _tourReservations;

        private string _guideUserName;
        public string GuideUserName
        {
            get { return _guideUserName; }
            set
            {
                _guideUserName = value;
                OnPropertyChanged(nameof(GuideUserName));

            }
        }
        private string _guideName;
        public string GuideName
        {
            get { return _guideName; }
            set
            {
                _guideName = value;
                OnPropertyChanged(nameof(GuideName));

            }
        }
        private string _guideLastName;
        public string GuideLastName
        {
            get { return _guideLastName; }
            set
            {
                _guideLastName = value;
                OnPropertyChanged(nameof(GuideLastName));

            }
        }
        private string _guideEmail;
        public string GuideEmail
        {
            get { return _guideEmail; }
            set
            {
                _guideEmail = value;
                OnPropertyChanged(nameof(GuideEmail));

            }
        }
        private string _guidePhoneNumber;
        public string GuidePhoneNumber
        {
            get { return _guidePhoneNumber; }
            set
            {
                _guidePhoneNumber = value;
                OnPropertyChanged(nameof(GuidePhoneNumber));

            }
        }


        public ICommand ResignCommand { get; set; }
        public ICommand SignOutCommand { get; }


        public GuideProfileViewModel(NavigationStore navigationStore, User user) 
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _userService = new UserService();

            GuideTours = new List<Tour>();
            _guestIds = new List<int>();
            _users = new List<User>(_userService.GetAll());
            _guests = new List<User>();

            foreach (Tour tour in _tourService.GetAll())
            {
                if (tour.GuideId == user.Id)
                {
                    GuideTours.Add(tour);
                }
            }

            _tourReservations = new List<TourReservation>(_tourReservationService.GetAll());


            foreach (TourReservation tourReservation in _tourReservations)
            {
                foreach(Tour tour in GuideTours)
                {
                    if (tourReservation.TourId == tour.Id && (tour.State == TourState.None || tour.State == TourState.Started))
                    {
                        _guestIds.Add(tourReservation.GuestId);
                        
                    }
                }
                
            }


            foreach (int id in _guestIds)
            {
                foreach (User u in _users)
                {
                    if (id == u.Id)
                    {
                        _guests.Add(u);
                    }

                }
            }


            _guideUserName = user.Username;
            _guideLastName = user.LastName;
            _guideEmail = user.Email;
            _guideName = user.FirstName;
            _guidePhoneNumber = user.PhoneNumber;

            ResignCommand = new ExecuteMethodCommand(Resign);
            SignOutCommand = new ExecuteMethodCommand(SignOut);



        }
        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void Resign()
        {
            MessageBoxResult result = MessageBox.Show("ARE YOU SURE YOU WANT TO RESIGN?", "RESIGN CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Tour t in GuideTours)
                {
                    if (t.Start > DateTime.Now)
                    {
                        t.State = TourState.Canceled;
                        _tourService.Update(t);
                    }

                }
                VoucherCreationView view = new VoucherCreationView(_navigationStore, _user, _guests, 2, true);
                view.Show();
                
            }
            else if (result == MessageBoxResult.No)
            {
                // Perform the action when "No" is clicked
                // Add your code here for the action you want to perform when "No" is clicked
            }

        }
    }
}
