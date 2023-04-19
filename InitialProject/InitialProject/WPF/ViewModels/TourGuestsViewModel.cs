using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourGuestsViewModel : ViewModelBase
    {
        private TourReservationService _tourReservationService;
        private UserService _userService;
        private TourService _tourService;

        private readonly ObservableCollection<TourReservation> _tourReservations;
        private readonly ObservableCollection<User> _users;
        private readonly ObservableCollection<User> _guests;

        public IEnumerable<User> Guests => _guests;

        private User _selectedGuest;
        public User SelectedGuest
        {
            get { return _selectedGuest; }
            set
            {
                _selectedGuest = value;
                OnPropertyChanged(nameof(SelectedGuest));
                GuestSelected();

            }
        }
        private void GuestSelected()
        {

            foreach (TourReservation res in _tourReservations)
            {
                if(res.GuestId == SelectedGuest.Id && res.TourId == _tour.Id)
                {
                    _tour.NumberOfArrivedGeusts += res.NumberOfGuests;
                    _tourService.Update(_tour);
                    res.Presence = Presence.Pending;
                    res.ArrivedAtKeyPoint = _tour.CurrentKeyPoint;
                    _tourReservationService.Update(res);
                    //BackNavigateCommand.Execute(null);
                }
            }
        }

        public ICommand BackCommand { get; set; }
        public ICommand BackNavigateCommand =>
new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        private readonly NavigationStore _navigationStore;
        private User _user;
        private Tour _tour;

        public TourGuestsViewModel(NavigationStore navigationStore, User user, Tour tour)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourReservationService = new TourReservationService();
            _userService = new UserService();
            _tourService = new TourService();
            _guests = new ObservableCollection<User>();
            _tour = tour;

            _tourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            _users = new ObservableCollection<User>(_userService.GetAll());

            foreach (TourReservation res in _tourReservations)
            {
                foreach (User u in _users)
                {
                    if (res.GuestId == u.Id && !_guests.Contains(u) && res.Presence == Presence.Absent && res.TourId == tour.Id)
                    {
                        _guests.Add(u);
                    }
                }
            }
            BackCommand = new ExecuteMethodCommand(Back);

        }
        private void Back()
        {
            BackNavigateCommand.Execute(null);
        }
        private TourLiveTrackingViewModel GoBack()
        {
            return new TourLiveTrackingViewModel(_navigationStore, _user, _tour);
        }
    }
}
