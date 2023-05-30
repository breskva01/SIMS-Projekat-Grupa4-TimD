using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand ResignCommand { get; set; }

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
                foreach(Tour tour in GuideTours )
                {
                    if (tourReservation.TourId == tour.Id)
                    {
                        _guestIds.Add(tourReservation.GuestId);
                    }
                }
                
            }

            foreach (int id in _guestIds)
            {
                foreach (User u in _users)
                {
                    if (id == u.Id && !_guests.Contains(u))
                    {
                        _guests.Add(u);
                    }

                }
            }


            _guideUserName = user.Username;

            ResignCommand = new ExecuteMethodCommand(Resign);

        }
        private void Resign()
        {
            foreach(Tour t in GuideTours)
            {
                t.State = TourState.Canceled;
                _tourService.Update(t);
            }
            VoucherCreationViewModel voucherCreationViewModel = new VoucherCreationViewModel(_navigationStore, _user,_guests, 2 );
        }
    }
}
