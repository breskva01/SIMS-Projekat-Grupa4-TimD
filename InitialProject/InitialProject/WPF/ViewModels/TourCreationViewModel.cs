﻿using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourCreationViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;
        private User _user;

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<KeyPoint> KeyPoints { get; set; }

        private List<KeyPoint> _tourKeyPoints = new List<KeyPoint>();
        private List<int> _keyPointIds = new List<int>();

        private LocationService _locationService;
        private KeyPointService _keyPointService;
        private TourService tourService;


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

        private List<string> _keyPointCities;
        public List<string> KeyPointCities
        {
            get { return _keyPointCities; }
            set
            {
                _keyPointCities = value;
                OnPropertyChanged(nameof(KeyPointCities));
            }
        }

        private List<string> _keyPointPlaces;
        public List<string> KeyPointPlaces
        {
            get { return _keyPointPlaces; }
            set
            {
                _keyPointPlaces = value;
                OnPropertyChanged(nameof(KeyPointPlaces));
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
                PopulateFirstKeyPointCityComboBox();

            }
        }
        
        private string _selectedKeyPointCity;
        public string SelectedKeyPointCity
        {
            get { return _selectedKeyPointCity; }
            set
            {
                _selectedKeyPointCity = value;
                OnPropertyChanged(nameof(SelectedKeyPointCity));
                PopulateKeyPointPlacesComboBox();

            }
        }

        private string _selectedKeyPointPlace;
        public string SelectedKeyPointPlace
        {
            get { return _selectedKeyPointPlace; }
            set
            {
                _selectedKeyPointPlace = value;
                OnPropertyChanged(nameof(SelectedKeyPointPlace));

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

        private void PopulateFirstKeyPointCityComboBox()
        {
            SelectedKeyPointCity = SelectedCity;
        }
        
        private void PopulateKeyPointPlacesComboBox()
        {
            if (string.IsNullOrEmpty(SelectedKeyPointCity))
            {
                KeyPointPlaces = null;
                return;
            }
            List<KeyPoint> keyPointsShow = new List<KeyPoint>();

            foreach (Location l in Locations)
            {
                if (SelectedKeyPointCity == l.City)
                {
                    foreach (KeyPoint ky in KeyPoints)
                    {
                        if (ky.LocationId == l.Id)
                        {
                            keyPointsShow.Add(ky);
                        }
                    }
                }
            }
            KeyPointPlaces = keyPointsShow.Select(l => l.Place).Distinct().ToList();
        }
        


        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _maximumGuests;
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
        private DateTime _start;
        public DateTime Start 
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _languageType;
        public string LanguageType
        {
            get => _languageType;
            set
            {
                if (value != _languageType)
                {
                    _languageType = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _pictureUrl;
        public string PictureUrl
        {
            get => _pictureUrl;
            set
            {
                if (value != _pictureUrl)
                {
                    _pictureUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _city;
        public string City
        {
            
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }

        }
        public ICommand ConfirmCommand;
        public ICommand CancelCommand { get; }
        public ICommand AddKeyPointCommand { get; } 

        public TourCreationViewModel(NavigationStore navigationStore, User user)
        {
            tourService = new TourService();    
            _locationService = new LocationService();
            _keyPointService = new KeyPointService();
            
           // ConfirmCommand = new CreateTourCommand(this);
            AddKeyPointCommand = new AddKeyPointCommand(this);

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            KeyPoints = new ObservableCollection<KeyPoint>(_keyPointService.GetAll());

            Countries = Locations.Select(l => l.Country).Distinct().ToList();
            KeyPointCities = Locations.Select(c => c.City).Distinct().ToList();

            Start = new DateTime(2022, 1, 1, 00, 5, 00);

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new ExecuteMethodCommand(CreateTour);

        }
        public void CreateTour()
        {
            Location Location = new Location();
            Location.Country = Country;
            Location.City = City;
            Location.Id = Locations.Where(c => c.City == City).Select(c => c.Id).FirstOrDefault();
            GuideLanguage lang = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), LanguageType);
            int TourDuration = int.Parse(Duration);
            int MaxGuests = int.Parse(MaximumGuests);

            foreach (KeyPoint ky in _tourKeyPoints)
            {
                _keyPointIds.Add(ky.Id);
            }

            tourService.CreateTour(TourName, Location, Description, lang, MaxGuests, Start, TourDuration, PictureUrl, _tourKeyPoints, _keyPointIds);

            // ovo u metodu
            TourName = null;
            City = null;
            Country = null;
            SelectedKeyPointCity = null;
            SelectedKeyPointPlace = null;
            Description = null;
            MaximumGuests = null;
            PictureUrl = null;
            Duration = null;
            LanguageType = null;
            _tourKeyPoints.Clear();
            _keyPointIds.Clear();
            
        }
        public void AddKeyPoint()
        {
            KeyPoint keyPoint = KeyPoints.Where(ky => ky.Place == SelectedKeyPointPlace).FirstOrDefault();

            _tourKeyPoints.Add(keyPoint);

            // Clearing comboboxes after adding one keyPoint
            SelectedKeyPointCity = null;
            SelectedKeyPointPlace = null;
        }


    }
}
