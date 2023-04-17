using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class AllToursViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private User _user;

        private LocationService _locationService;
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private VoucherService _voucherService; 

        public ICommand CancelTourCommand { get; set; }

        private DateTime _today;

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));

            }
        }

        private List<User> _users;
        private List<int> _guestIds;
        private List<User> _guests;
        private List<TourReservation> _tourReservations;
        private UserService _userService;

        public ICommand CreateVoucherNavigateCommand =>
   new NavigateCommand(new NavigationService(_navigationStore, CreateVoucher()));

        public AllToursViewModel(NavigationStore navigationStore, User user)
        {
            _toursShow = new ObservableCollection<Tour>();

            _navigationStore = navigationStore;
            _user = user;

            _voucherService = new VoucherService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _userService = new UserService();
            _tourReservationService = new TourReservationService();

            _guestIds = new List<int>();
            _guests = new List<User>();

            _tourReservations = new List<TourReservation>(_tourReservationService.GetAll());
            _tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            _users = new List<User>(_userService.GetAll());



            foreach (Tour t in _tours)
            {
                foreach (Location l in Locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            foreach (Tour t in _tours)
            {
                if (t.State == 0)
                {
                    _toursShow.Add(t);
                }
            }

            _today = DateTime.Now;

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            CancelTourCommand = new ExecuteMethodCommand(CancelTour);
        }
        private void CancelTour()
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");

                return;
            }
            if (SelectedTour.Start > _today.AddHours(48))
            {
                if (SelectedTour.State == TourState.None)
                {
                    SelectedTour.State = TourState.Canceled;

                    foreach (TourReservation tourReservation in _tourReservations)
                    {
                        if (tourReservation.TourId == SelectedTour.Id)
                        {
                            _guestIds.Add(tourReservation.GuestId);
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

                    CreateVoucherNavigateCommand.Execute(null);

                    /*
                    foreach (User guest in _guests)
                    {
                        guest.VouchersIds.Add();
                    }
                    */


                    _tourService.Update(SelectedTour);

                    _toursShow.Remove(SelectedTour);
                    return;
                }
            }
            MessageBox.Show("It's too late to cancel this tour.");
            return;
        }
        
        private VoucherCreationViewModel CreateVoucher()
        {
            return new VoucherCreationViewModel(_navigationStore, _user, _guests);

        }
        
    }
}
