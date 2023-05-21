using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationRegistrationViewModel: ViewModelBase
    {
        public List<Location> Locations { get; set; }
        private readonly User _loggedInUser;
        private readonly AccommodationService _accommodationService;
        private readonly LocationService _locationService;
        private readonly NavigationStore _navigationStore;

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }
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
            get => _selectedCountry;
            set
            {
                if (value != _selectedCountry)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                    PopulateCitiesComboBox();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (value != _selectedCity)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationType _type;
        public AccommodationType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maximumGuests = "1";
        public string MaximumGuests
        {
            get => _maximumGuests;
            set
            {
                if (value != _maximumGuests)
                {
                    _maximumGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _minimumDays = "1";
        public string MinimumDays
        {
            get => _minimumDays;
            set
            {
                if (value != _minimumDays)
                {
                    _minimumDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _minimumCancellationNotice = "1";
        public string MinimumCancellationNotice
        {
            get => _minimumCancellationNotice;
            set
            {
                if (value != _minimumCancellationNotice)
                {
                    _minimumCancellationNotice = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pictureURL;
        public string PictureURL
        {
            get => _pictureURL;
            set
            {
                if (value != _pictureURL)
                {
                    _pictureURL = value;
                    OnPropertyChanged();
                }
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

        public ICommand BackCommand { get; set; }
        public ICommand RegisterAccommodationCommand { get; set; }
        public ICommand BackNavigateCommand =>
        new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        public AccommodationRegistrationViewModel(NavigationStore navigationStore ,User user) 
        {
            _loggedInUser = user;
            _accommodationService = new AccommodationService();
            _locationService = new LocationService();
            _navigationStore = navigationStore;
            Locations = new List<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();
            Cities = Locations.Select(c => c.City).Distinct().ToList();
            InitializeCommands();
        }
        private void InitializeCommands() 
        {
            RegisterAccommodationCommand = new ExecuteMethodCommand(RegisterAccommodation);
            BackCommand = new ExecuteMethodCommand(Back);
        }
        private void RegisterAccommodation() 
        {
            int maximumGuests = int.Parse(MaximumGuests);
            int minimumDays = int.Parse(MinimumDays);
            int minimumCancellationNotice = int.Parse(MinimumCancellationNotice);
            _accommodationService.RegisterAccommodation(AccommodationName, SelectedCountry, SelectedCity, Address, Type, maximumGuests, minimumDays, minimumCancellationNotice, 
                PictureURL, _loggedInUser);
            AccommodationName = null;
            SelectedCountry = null;
            SelectedCity = null;
            Address = null;
            MaximumGuests = "1";
            MinimumDays = "1";
            MinimumCancellationNotice = "1";
            PictureURL = null;
        }
        private void Back()
        {
            BackNavigateCommand.Execute(null);
        }
        private OwnerViewModel GoBack()
        {
            return new OwnerViewModel(_navigationStore, _loggedInUser);
        }
    }
}
