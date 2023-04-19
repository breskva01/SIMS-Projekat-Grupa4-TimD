using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class TourStatsViewModel : ViewModelBase
    {
        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private User _user;

        private LocationService _locationService;

        private Tour tour;

        private readonly ObservableCollection<Tour> _tours;
        public IEnumerable<Tour> MostVisited => _tours;

        private TourService _tourService;
        private TourReservationService _tourReservationService;


        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                YearsSelectionChanged();

            }
        }

        private string _selectedTourName;
        public string SelectedTourName
        {
            get { return _selectedTourName; }
            set
            {
                _selectedTourName = value;
                OnPropertyChanged(nameof(SelectedTourName));
                NamesSelectionChanged();

            }
        }

        private string _allGuests;
        public string AllGuests
        {
            get { return _allGuests; }
            set
            {
                _allGuests = value;
                OnPropertyChanged(nameof(AllGuests));

            }
        }

        private List<string> _tourNames;
        public List<string> TourNames
        {
            get { return _tourNames; }
            set
            {
                _tourNames = value;
                OnPropertyChanged(nameof(TourNames));
            }
        }

        private List<string> _avaiableYears;
        public List<string> AvaiableYears
        {
            get { return _avaiableYears; }
            set
            {
                _avaiableYears = value;
                OnPropertyChanged(nameof(AvaiableYears));
            }
        }

        private void YearsSelectionChanged()
        {
            _tours.Clear();
            tour = _tourService.GetMostVisited(SelectedYear);

            foreach (Location l in Locations)
            {
                if (tour.LocationId == l.Id)
                    tour.Location = l;
            }
            _tours.Add(tour);
        }
        
        private string _minorGuests;
        public string MinorGuests
        {
            get { return _minorGuests; }
            set
            {
                _minorGuests = value;
                OnPropertyChanged(nameof(MinorGuests));

            }
        }

        private string _middleAgeGuests;
        public string MiddleAgeGuests
        {
            get { return _middleAgeGuests; }
            set
            {
                _middleAgeGuests = value;
                OnPropertyChanged(nameof(MiddleAgeGuests));

            }
        }
        private string _withVoucher;
        public string WithVoucher
        {
            get { return _withVoucher; }
            set
            {
                _withVoucher = value;
                OnPropertyChanged(nameof(WithVoucher));

            }
        }

        private string _withoutVoucher;
        public string WithoutVoucher
        {
            get { return _withoutVoucher; }
            set
            {
                _withoutVoucher = value;
                OnPropertyChanged(nameof(WithoutVoucher));

            }
        }

        private string _olderGuests;
        public string OlderGuests
        {
            get { return _olderGuests; }
            set
            {
                _olderGuests = value;
                OnPropertyChanged(nameof(OlderGuests));

            }
        }


        private void NamesSelectionChanged()
        {
            Tour tour = new Tour();
            tour = _tourService.GetByName(SelectedTourName);
            AllGuests = tour.NumberOfArrivedGeusts.ToString();
            MinorGuests = _tourService.GetNumberOfGuestBelow18(tour);
            MiddleAgeGuests = _tourService.GetNumberOfMiddleAgeGuests(tour);
            OlderGuests = _tourService.GetNumberOfOlderGuests(tour);
            WithVoucher = _tourReservationService.GetVoucherPercentage(tour.Id);
            WithoutVoucher = (100 - int.Parse(WithVoucher)).ToString();
            WithVoucher += "%";
            WithoutVoucher += "%";

        }


        public TourStatsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tours = new ObservableCollection<Tour>();

            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _locationService = new LocationService();

            tour = new Tour();
            _avaiableYears = new List<string>();
            _tourNames = new List<string>();

            AllGuests = string.Empty;

            _tourNames = _tourService.GetFinishedTourNames();
            _avaiableYears = _tourService.GetAvailableYears();
            _avaiableYears.Add("All time");

            Locations = new ObservableCollection<Location>(_locationService.GetAll());


            
        }
    }
}
