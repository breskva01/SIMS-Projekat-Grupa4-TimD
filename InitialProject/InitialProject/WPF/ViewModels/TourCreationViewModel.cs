using ControlzEx.Standard;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourCreationViewModel : ViewModelBase, IDataErrorInfo
    {

        private bool IsCreatedBasedOnStats;
        private bool IsCreatedBasedOnSignleRequest;

        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourRequest TourRequest { get; set; }

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<KeyPoint> KeyPoints { get; set; }

        private List<KeyPoint> _tourKeyPoints = new List<KeyPoint>();
        private List<int> _keyPointIds = new List<int>();

        private LocationService _locationService;
        private KeyPointService _keyPointService;
        private TourService _tourService;
        private TourRequestService _tourRequestService;
        private UserNotificationService _userNotificationService;

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand AddKeyPointCommand { get; }
        public ICommand BackCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
        public ICommand IncreaseDurationCommand { get; set; }
        public ICommand DecreaseDurationCommand { get; set; }
        public ICommand IncreaseGuestsCommand { get; set; }
        public ICommand DecreaseGuestsCommand { get; set; }

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
        private int _maximumGuests;
        public int MaximumGuests
        {
            get => _maximumGuests;
            set
            {
                if (value != _maximumGuests && value >= 0)
                {
                    _maximumGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _start;
        public string Start
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
        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration && value >= 0)
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

        public TourCreationViewModel(NavigationStore navigationStore, User user, string parameter, bool isParameterLanguage)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourService = new TourService();
            _locationService = new LocationService();
            _keyPointService = new KeyPointService();
            _tourRequestService = new TourRequestService();
            _userNotificationService = new UserNotificationService();

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            KeyPoints = new ObservableCollection<KeyPoint>(_keyPointService.GetAll());
            KeyPointCities = Locations.Select(c => c.City).Distinct().ToList();

            if(isParameterLanguage == true)
            {
                LanguageType = parameter;
            }
            else
            {
                string[] parts = parameter.Split(' ');
                Country = parts[0];
                City = string.Join(" ", parts.Skip(1));
                SelectedCity = City;
            }

            AddKeyPointCommand = new AddKeyPointCommand(this);


            InitializeCommands();

            IsCreatedBasedOnStats = true;
            IsCreatedBasedOnSignleRequest = false;


        }
        public TourCreationViewModel(NavigationStore navigationStore, User user, TourRequest request)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourService = new TourService();
            _locationService = new LocationService();
            _keyPointService = new KeyPointService();
            _tourRequestService = new TourRequestService();
            _userNotificationService = new UserNotificationService();

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            KeyPoints = new ObservableCollection<KeyPoint>(_keyPointService.GetAll());
            KeyPointCities = Locations.Select(c => c.City).Distinct().ToList();

            Country = request.Location.Country;
            City = request.Location.City;
            SelectedCity = request.Location.City;
            LanguageType = request.Language.ToString();
            MaximumGuests = request.NumberOfGuests;
            Description = request.Description;

            AddKeyPointCommand = new AddKeyPointCommand(this);

            TourRequest = request;

            InitializeCommands();

            IsCreatedBasedOnStats = false;
            IsCreatedBasedOnSignleRequest = true;

        }

        public TourCreationViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourService = new TourService();
            _locationService = new LocationService();
            _keyPointService = new KeyPointService();
            _userNotificationService = new UserNotificationService();


            AddKeyPointCommand = new AddKeyPointCommand(this);

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            KeyPoints = new ObservableCollection<KeyPoint>(_keyPointService.GetAll());

            Countries = Locations.Select(l => l.Country).Distinct().ToList();
            KeyPointCities = Locations.Select(c => c.City).Distinct().ToList();

            InitializeCommands();

            IsCreatedBasedOnStats = false;
            IsCreatedBasedOnSignleRequest = false;


            //Start = new DateTime(2023, 4, 15);
        }

        private void InitializeCommands()
        {
            ConfirmCommand = new ExecuteMethodCommand(CreateTour);
            //BackCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            HomeCommand = new ExecuteMethodCommand(ShowGuideMenuView);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            SignOutCommand = new ExecuteMethodCommand(SignOut);
            IncreaseDurationCommand = new ExecuteMethodCommand(IncreaseDuration);
            DecreaseDurationCommand = new ExecuteMethodCommand(DecreaseDuration);
            IncreaseGuestsCommand = new ExecuteMethodCommand(IncreaseGuests);
            DecreaseGuestsCommand = new ExecuteMethodCommand(DecreaseGuests);
        }
        public void CreateTour()
        {
            Location Location = new Location();
            Location.Country = Country;
            Location.City = City;
            Location.Id = Locations.Where(c => c.City == City).Select(c => c.Id).FirstOrDefault();
            GuideLanguage lang = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), LanguageType);
            //int TourDuration = int.Parse(Duration);
            // int MaxGuests = int.Parse(MaximumGuests);
            int MaxGuests = MaximumGuests;
            int TourDuration = Duration;

            foreach (KeyPoint ky in _tourKeyPoints)
            {
                _keyPointIds.Add(ky.Id);
            }

            Tour tour = _tourService.CreateTour(TourName, Location, Description, lang, MaxGuests, Convert.ToDateTime(Start), TourDuration, PictureUrl, _tourKeyPoints, _keyPointIds);

            if(TourRequest != null)
            {
                TourRequest.TourId = tour.Id;
                _tourRequestService.Update(TourRequest);
            }
            if(IsCreatedBasedOnStats)
            {
                _userNotificationService.NotifySimilarRequests(tour);
            }
            if(IsCreatedBasedOnSignleRequest)
            {
                _userNotificationService.NotifyApprovedRequest(tour, TourRequest.UserId);
            }

            ClearOutTextBoxes();

        }
        private void ClearOutTextBoxes()
        {
            TourName = null;
            City = null;
            Country = null;
            SelectedKeyPointCity = null;
            SelectedKeyPointPlace = null;
            Description = null;
            MaximumGuests = 0;
            PictureUrl = null;
            Duration = 0;
            LanguageType = null;
            Start = null;
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

        private void ShowGuideMenuView()
        {
            GuideMenuViewModel viewModel = new GuideMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
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
        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void ShowTourCreationView()
        {
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowToursTodayView()
        {
            ToursTodayViewModel viewModel = new ToursTodayViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourCancellationView()
        {
            AllToursViewModel viewModel = new AllToursViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourStatsView()
        {
            TourStatsViewModel viewModel = new TourStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideRatingsView()
        {
            GuideRatingsViewModel viewModel = new GuideRatingsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        private void IncreaseDuration()
        {
            Duration++;
        }
        private void DecreaseDuration()
        {
            Duration--;
        }
        private void IncreaseGuests()
        {
            MaximumGuests++;
        }
        private void DecreaseGuests()
        {
            MaximumGuests--;
        }

        public string this[string columnName]
        {
            get
            {
                string? error = null;
                string requiredMessage = "Obavezno polje";
                switch (columnName)
                {
                    case nameof(Start):
                        if (TourRequest != null)
                        {
                            if (Convert.ToDateTime(Start) < TourRequest.EarliestDate || Convert.ToDateTime(Start) > TourRequest.LatestDate) error = "NOPE";
                        }
                        break;
                    default:
                        break;
                }
                return error;

            }
        }
        public string Error => null;
        public bool IsTourValid
        {
            get
            {
                foreach (var property in new string[]
                {
                    nameof(Start) })
                {
                    if (this[property] != null) return false;
                }
                return true;
            }

        }
    }
}
