using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.ViewModels
{
    public class TourBrowserViewModel : ViewModelBase
    {
        //private readonly ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }

        public ObservableCollection<Location> Locations { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;

        private List<string> _countries;
        public List<string> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }

        private List<string> _cities;
        public List<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                PopulateCitiesComboBox();
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));

            }
        }
        private void PopulateCitiesComboBox()
        {
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                Cities = null;
                return;
            }
            Cities = Locations.Where(l => l.Country == SelectedCountry).Select(l => l.City).ToList();
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));

            }
        }

        private int _selectedLanguageIndex;
        public int SelectedLanguageIndex
        {
            get { return _selectedLanguageIndex; }
            set
            {
                _selectedLanguageIndex = value;
                OnPropertyChanged(nameof(SelectedLanguageIndex));
            }
        }

        private string _numberOfGuests;
        public string NumberOfGuests
        {
            get { return _numberOfGuests; }
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged(nameof(NumberOfGuests));

            }
        }

        private bool _isSortNameChecked;
        public bool IsSortNameChecked
        {
            get { return _isSortNameChecked; }
            set
            {
                _isSortNameChecked = value;
                OnPropertyChanged(nameof(IsSortNameChecked));

            }
        }

        private bool _isSortLocationChecked;
        public bool IsSortLocationChecked
        {
            get { return _isSortLocationChecked; }
            set
            {
                _isSortLocationChecked = value;
                OnPropertyChanged(nameof(IsSortLocationChecked));

            }
        }

        private bool _isSortDurationChecked;
        public bool IsSortDurationChecked
        {
            get { return _isSortDurationChecked; }
            set
            {
                _isSortDurationChecked = value;
                OnPropertyChanged(nameof(IsSortDurationChecked));

            }
        }

        private bool _isSortLanguageChecked;
        public bool IsSortLanguageChecked
        {
            get { return _isSortLanguageChecked; }
            set
            {
                _isSortLanguageChecked = value;
                OnPropertyChanged(nameof(IsSortLanguageChecked));

            }
        }

        public ICommand FilterCommand { get; }
        public ICommand ResetCommand { get;  }
        public ICommand SortCommand { get;  }
        public ICommand MakeReservationCommand { get; }
        public ICommand MenuCommand { get; }

        public TourBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            //Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            _tourService = new TourService();
            _locationService = new LocationService();

            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            foreach (Tour t in Tours)
            {
                t.Location = Locations.FirstOrDefault(l => l.Id == t.LocationId);
            }
            Countries = Locations.Select(l => l.Country).Distinct().ToList();

            FilterCommand = new ExecuteMethodCommand(ApplyFilter);
            ResetCommand = new ExecuteMethodCommand(ResetFilter);
            SortCommand = new ExecuteMethodCommand(ApplySort);
            MakeReservationCommand = new TourClickCommand(ShowTourReservationView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowTourReservationView(Tour tour)
        {
            if (tour.CurrentNumberOfGuests == tour.MaximumGuests)
            {
                MessageBox.Show("Unfortunately, the tour that you are interested in is fully booked. On the previous window you can take a look at other tours that are located in the same location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                Tours.Clear();
                foreach (var t in _tourService.GetFiltered(tour.Location.Country, tour.Location.City, 0, GuideLanguage.All, 1))
                {
                    t.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                    Tours.Add(t);
                }

                return;
            }

            var viewModel = new TourReservationViewModel(_navigationStore, _user, tour);
            var reserveTourNavigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            reserveTourNavigateCommand.Execute(null);
        }

        private void ApplyFilter()
        {
            string country = SelectedCountry;
            string city = SelectedCity;
            int duration = GetDuration();
            GuideLanguage language = GetLanguage();
            int numberOfGuests = GetNumberOfGuests();

            Tours.Clear();
            foreach (var tour in _tourService.GetFiltered(country, city, duration, language, numberOfGuests))
            {
                tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }
        }

        private GuideLanguage GetLanguage()
        {
            switch (SelectedLanguageIndex)
            {
                case 0:
                    return GuideLanguage.All;
                case 1:
                    return GuideLanguage.Serbian;
                default:
                    return GuideLanguage.English;
            }
        }

        private int GetNumberOfGuests()
        {
            int numberOfGuests = 0;

            try
            {
                numberOfGuests = int.Parse(NumberOfGuests);
            }
            catch { 
                MessageBox.Show("You entered a non-number value for number of guests.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            return numberOfGuests;
        }

        private int GetDuration()
        {
            int duration = 0;

            try
            {
                duration = int.Parse(Duration);
            }
            catch {
                MessageBox.Show("You entered a non-number value for duration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            return duration;
        }
        private void ResetFilter()
        {
            SelectedCountry = "";
            SelectedCity = "";
            Duration="0";
            SelectedLanguageIndex = 0;
            NumberOfGuests= "0";

            Tours.Clear();
            foreach (var tour in _tourService.GetAll())
            {
                tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }

        }
        private void ApplySort()
        {
            if (IsSortNameChecked)
            {
                var sortedTours = _tourService.SortByName(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (IsSortLocationChecked)
            {
                var sortedTours = _tourService.SortByLocation(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (IsSortDurationChecked)
            {
                var sortedTours = _tourService.SortByDuration(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (IsSortLanguageChecked)
            {
                var sortedTours = _tourService.SortByLanguage(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }

        }

    }
}
