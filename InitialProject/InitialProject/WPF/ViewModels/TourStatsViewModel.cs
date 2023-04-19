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
    public class TourStatsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;
        private Tour _tour;

        public ObservableCollection<Location> Locations { get; set; }

        private readonly ObservableCollection<Tour> _tours;
        public IEnumerable<Tour> MostVisited => _tours;

        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private LocationService _locationService;

        public ICommand BackCommand { get; set; }

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

        public TourStatsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tour = new Tour();
            _tours = new ObservableCollection<Tour>();
            _avaiableYears = new List<string>();
            _tourNames = new List<string>();

            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _locationService = new LocationService();

            AllGuests = string.Empty;

            _tourNames = _tourService.GetFinishedTourNames();
            _avaiableYears = _tourService.GetAvailableYears();
            _avaiableYears.Add("All time");

            Locations = new ObservableCollection<Location>(_locationService.GetAll());

            InitializeCommands();
        }

        private void InitializeCommands()
        { 
            BackCommand = new ExecuteMethodCommand(ShowGuideMenuView);

        }

        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }

        private void YearsSelectionChanged()
        {
            _tours.Clear();
            _tour = _tourService.GetMostVisited(SelectedYear);

            foreach (Location l in Locations)
            {
                if (_tour.LocationId == l.Id)
                    _tour.Location = l;
            }
            _tours.Add(_tour);
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

    }
}
