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
using LiveCharts;
using LiveCharts.Wpf;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsStatsViewModel : ViewModelBase
    {

        private SeriesCollection _seriesCollectionYearLocation;
        public SeriesCollection SeriesCollectionYearLocation
        {
            get { return _seriesCollectionYearLocation; }
            set
            {
                _seriesCollectionYearLocation = value;
                OnPropertyChanged(nameof(SeriesCollectionYearLocation));
            }
        }
        private SeriesCollection _seriesCollectionMonthLocation;
        public SeriesCollection SeriesCollectionMonthLocation
        {
            get { return _seriesCollectionMonthLocation; }
            set
            {
                _seriesCollectionMonthLocation = value;
                OnPropertyChanged(nameof(SeriesCollectionMonthLocation));
            }
        }
        private SeriesCollection _seriesCollectionYearLanguage;
        public SeriesCollection SeriesCollectionYearLanguage
        {
            get { return _seriesCollectionYearLanguage; }
            set
            {
                _seriesCollectionYearLanguage = value;
                OnPropertyChanged(nameof(SeriesCollectionYearLanguage));
            }
        }
        private SeriesCollection _seriesCollectionMonthLanguage;
        public SeriesCollection SeriesCollectionMonthLanguage
        {
            get { return _seriesCollectionMonthLanguage; }
            set
            {
                _seriesCollectionMonthLanguage = value;
                OnPropertyChanged(nameof(SeriesCollectionMonthLanguage));
            }
        }

        private bool _isFirstLocationGraphVisible;
        public bool IsFirstLocationGraphVisible
        {
            get { return _isFirstLocationGraphVisible; }
            set
            {
                _isFirstLocationGraphVisible = value;
                OnPropertyChanged(nameof(IsFirstLocationGraphVisible));
            }
        }

        private bool _isSecondLocationGraphVisible;
        public bool IsSecondLocationGraphVisible
        {
            get { return _isSecondLocationGraphVisible; }
            set
            {
                _isSecondLocationGraphVisible = value;
                OnPropertyChanged(nameof(IsSecondLocationGraphVisible));
            }
        }

        private bool _isFirstLanguageGraphVisible;
        public bool IsFirstLanguageGraphVisible
        {
            get { return _isFirstLanguageGraphVisible; }
            set
            {
                _isFirstLanguageGraphVisible = value;
                OnPropertyChanged(nameof(IsFirstLanguageGraphVisible));
            }
        }

        private bool _isSecondLanguageGraphVisible;
        public bool IsSecondLanguageGraphVisible
        {
            get { return _isSecondLanguageGraphVisible; }
            set
            {
                _isSecondLanguageGraphVisible = value;
                OnPropertyChanged(nameof(IsSecondLanguageGraphVisible));
            }
        }

        public List<int> Years { get; set; }
        public List<string> Months { get; set; }

        public List<int> YearNumberOfRequests { get; set; }
        public List<int> MonthsNumberOfRequests { get; set; }

        private string[] _yearsAxes;
        public string[] YearsAxes
        {
            get { return _yearsAxes; }
            set
            {
                _yearsAxes = value;
                OnPropertyChanged(nameof(YearsAxes));
            }
        }
        private string[] _monthsAxes;
        public string[] MonthsAxes
        {
            get { return _monthsAxes; }
            set
            {
                _monthsAxes = value;
                OnPropertyChanged(nameof(MonthsAxes));
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

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));

            }
        }

        private bool _isCheckedYears;
        public bool IsCheckedYears
        {
            get { return _isCheckedYears; }
            set
            {
                 _isCheckedYears = value;
                OnPropertyChanged(nameof(IsCheckedYears));
                if (_isCheckedYears == true)
                {
                    if(SelectedCity != null && SelectedCountry != null && SelectedLanguage == null)
                    {
                        InitializeLocationYearsGraph();
                    }
                    else if(SelectedLanguage != null && SelectedCountry == null && SelectedCity == null)
                    {
                        InitializeLanguageYearsGraph();
                    }
                    
                }
            }
        }
        private bool _isCheckedMonths;
        public bool IsCheckedMonths
        {
            get { return _isCheckedMonths; }
            set
            {
                _isCheckedMonths = value;
                OnPropertyChanged(nameof(IsCheckedMonths));
                if (_isCheckedMonths == true)
                {
                    PopulateYearsComboBox();
                }
            }
        }

        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                if (SelectedCity != null && SelectedCountry != null && SelectedLanguage == null)
                {
                    InitializeLocationMonthsGraph();
                }
                else if (SelectedLanguage != null && SelectedCountry == null && SelectedCity == null)
                {
                    InitializeLanguageMonthsGraph();
                }
            }
        }
        private List<string> _allYears;
        public List<string> AllYears
        {
            get { return _allYears; }
            set
            {
                _allYears = value;
                OnPropertyChanged(nameof(AllYears));
            }
        }

        private bool _isCheckedLocation;
        public bool IsCheckedLocation
        {
            get { return _isCheckedLocation; }
            set
            {
                _isCheckedLocation = value;
                if(_isCheckedLocation == true)
                {
                    OnPropertyChanged(nameof(IsCheckedLocation));
                    PopulateLocationTextBox();
                }
               
            }
        }
        private bool _isCheckedLanguage;
        public bool IsCheckedLanguage
        {
            get { return _isCheckedLanguage; }
            set
            {
                _isCheckedLanguage = value;
                if(_isCheckedLanguage == true)
                {
                    OnPropertyChanged(nameof(IsCheckedLanguage));
                    PopulateLanguageTextBox();
                }
               
            }
        }
        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TourRequest> Requests { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public List<string> Languages { get; set; }

        private TourRequestService _tourRequestService;
        private LocationService _locationService;

        private readonly NavigationStore _navigationStore;
        private User _user;

        public Func<double, string> YFormatter { get; }

        public ICommand CreateCommand { get; set; }
        public ICommand ResetCommand { get; set; }


        public TourRequestsStatsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            YFormatter = value => Math.Round(value, 2).ToString();

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            Requests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());

            foreach (TourRequest tourRequest in Requests)
            {
                tourRequest.Location = _locationService.GetById(tourRequest.Location.Id);
            }
            CreateCommand = new ExecuteMethodCommand(CreateTour);
            ResetCommand = new ExecuteMethodCommand(ResetFilters);

            YearsAxes = new string[] { };
            MonthsAxes = new string[] { };
            Months = new List<string>();
            Years = new List<int>();

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();

            _seriesCollectionYearLocation = new SeriesCollection();
            _seriesCollectionMonthLocation = new SeriesCollection();
            _seriesCollectionYearLanguage = new SeriesCollection();
            _seriesCollectionMonthLanguage = new SeriesCollection();

            AllYears = new List<string>();

            Languages = new List<string>();
            Languages.Add("Serbian");
            Languages.Add("English");

            Months.Add("January");
            Months.Add("February");
            Months.Add("March");
            Months.Add("April");
            Months.Add("May");
            Months.Add("June");
            Months.Add("July");
            Months.Add("August");
            Months.Add("September");
            Months.Add("October");
            Months.Add("November");
            Months.Add("December");

        }
        private void PopulateLocationTextBox()
        {
            Language = null;

            var tourRequestCounts = Requests
           .GroupBy(r => r.Location)
           .Select(group => new
           {
               Location = group.Key,
               Count = group.Count()
           })
           .ToList();

            var locationWithMostTours = tourRequestCounts
           .OrderByDescending(item => item.Count)
           .FirstOrDefault();

            Location = locationWithMostTours.Location.Country + " " + locationWithMostTours.Location.City;

        }
        private void PopulateLanguageTextBox()
        {
            Location = null;

            var tourRequestCounts = Requests
          .GroupBy(r => r.Language)
          .Select(group => new
          {
              Language = group.Key,
              Count = group.Count()
          })
          .ToList();

            var languageWithMostTours = tourRequestCounts
           .OrderByDescending(item => item.Count)
           .FirstOrDefault();

            Language = languageWithMostTours.Language.ToString();     
        }
        private void PopulateYearsComboBox()
        {
            SeriesCollectionYearLocation.Clear();
            List<int> AllYearsInt = _tourRequestService.GetAllYears();
            foreach (int year in AllYearsInt)
            {
                AllYears.Add(year.ToString());
            }
        }
        private void InitializeLocationYearsGraph()
        {
            IsFirstLocationGraphVisible = true;
            IsSecondLocationGraphVisible = false;

            SeriesCollectionYearLocation.Clear();
            SeriesCollectionMonthLocation.Clear();

            List<TourRequest> allTourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in allTourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }

            List<TourRequest> requests = _tourRequestService.GetForChosenLocation(allTourRequests, SelectedCountry, SelectedCity);

            Years = _tourRequestService.GetRangeOfYears(requests);
            YearsAxes = new string[Years.Count];
            YearNumberOfRequests = new List<int>();

            foreach (int year in Years)
            {
                YearNumberOfRequests.Add(_tourRequestService.GetYearNumberOfRequestsForChosenLocation(year, SelectedCountry, SelectedCity, requests));
            }
            int i = 0;
            foreach (int year in Years)
            {
                YearsAxes[i] = year.ToString();
                i++;
            }

            LineSeries YearNumberOfRequestAxes = new LineSeries();
            YearNumberOfRequestAxes.Title = "yearRequests";
            YearNumberOfRequestAxes.Values = new ChartValues<int>(YearNumberOfRequests);
            YearNumberOfRequestAxes.LineSmoothness = 0;
            SeriesCollectionYearLocation = new SeriesCollection();
            SeriesCollectionYearLocation.Add(YearNumberOfRequestAxes);


        }
        private void InitializeLocationMonthsGraph()
        {
            IsFirstLocationGraphVisible = false;
            IsSecondLocationGraphVisible = true;

            SeriesCollectionYearLocation.Clear();
            SeriesCollectionMonthLocation.Clear();
            List<TourRequest> allTourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in allTourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }

            List<TourRequest> requests = _tourRequestService.GetForChosenLocation(allTourRequests, SelectedCountry, SelectedCity);


            MonthsAxes = new string[Months.Count];
            MonthsNumberOfRequests = new List<int>();

            for (int j = 1; j <= Months.Count(); j++)
            {
                MonthsNumberOfRequests.Add(_tourRequestService.GetMonthNumberOfRequests(j, SelectedCountry, SelectedCity,Convert.ToInt32(SelectedYear), requests)); 
            }

            int i = 0;
            
            foreach (string month in Months)
            {
                MonthsAxes[i] = month;
                i++;
            }

            LineSeries MonthNumberOfRequestAxes = new LineSeries();
            MonthNumberOfRequestAxes.Title = "monthsRequests";
            MonthNumberOfRequestAxes.Values = new ChartValues<int>(MonthsNumberOfRequests);
            MonthNumberOfRequestAxes.LineSmoothness = 0;
            SeriesCollectionMonthLocation = new SeriesCollection();
            SeriesCollectionMonthLocation.Add(MonthNumberOfRequestAxes);


        }

        private void InitializeLanguageYearsGraph()
        {

            IsFirstLanguageGraphVisible = true;
            IsSecondLanguageGraphVisible = false;

            List<TourRequest> allTourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in allTourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }

            List<TourRequest> requests = _tourRequestService.GetForChosenLanguage(allTourRequests, SelectedLanguage.ToString());

            Years = _tourRequestService.GetRangeOfYears(requests);
            YearsAxes = new string[Years.Count];
            YearNumberOfRequests = new List<int>();

            foreach (int year in Years)
            {
                YearNumberOfRequests.Add(_tourRequestService.GetYearNumberOfRequestsForChosenLanguage(year, SelectedLanguage, requests));
            }
            int i = 0;
            foreach (int year in Years)
            {
                YearsAxes[i] = year.ToString();
                i++;
            }
            // za izabranu lokaciju treba da nadjem sve godine requestova

            LineSeries YearNumberOfRequestAxes = new LineSeries();
            YearNumberOfRequestAxes.Title = "yearRequests";
            YearNumberOfRequestAxes.Values = new ChartValues<int>(YearNumberOfRequests);
            YearNumberOfRequestAxes.LineSmoothness = 0;
            SeriesCollectionYearLanguage = new SeriesCollection();
            SeriesCollectionYearLanguage.Add(YearNumberOfRequestAxes);


        }
        private void InitializeLanguageMonthsGraph()
        {
            IsFirstLanguageGraphVisible = false;
            IsSecondLanguageGraphVisible = true;

            SeriesCollectionYearLanguage.Clear();
            List<TourRequest> allTourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in allTourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }

            List<TourRequest> requests = _tourRequestService.GetForChosenLanguage(allTourRequests, SelectedLanguage);

            MonthsAxes = new string[Months.Count];
            MonthsNumberOfRequests = new List<int>();

            for (int j = 1; j <= Months.Count(); j++)
            {
                MonthsNumberOfRequests.Add(_tourRequestService.GetMonthNumberOfRequestsLanguage(j, SelectedLanguage, Convert.ToInt32(SelectedYear), requests));
            }

            int i = 0;
            foreach (string month in Months)
            {
                MonthsAxes[i] = month;
                i++;
            }

            LineSeries MonthNumberOfRequestAxes = new LineSeries();
            MonthNumberOfRequestAxes.Title = "monthsRequests";
            MonthNumberOfRequestAxes.Values = new ChartValues<int>(MonthsNumberOfRequests);
            MonthNumberOfRequestAxes.LineSmoothness = 0;
            SeriesCollectionMonthLanguage = new SeriesCollection();
            SeriesCollectionMonthLanguage.Add(MonthNumberOfRequestAxes);

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

        private void CreateTour()
        {
            
            if (Language == null && Location != null)
            {
                bool isParameterLanguage = false;
                TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user, Location, isParameterLanguage);
                NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
                navigate.Execute(null);
            }
            else if (Language != null && Location == null) 
            {
                bool isParameterLanguage = true;
                TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user, Language, isParameterLanguage);
                NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
                navigate.Execute(null);
            }
            else
            {
                
            }
            
        }
        private void ResetFilters()
        {
            SelectedLanguage = null;
            SelectedYear = null;
            SelectedCountry = null;
            SelectedCity = null;
            IsCheckedMonths = false;
            IsCheckedYears = false;
            AllYears.Clear();
            
            SeriesCollectionMonthLanguage.Clear();
            SeriesCollectionMonthLocation.Clear();
            SeriesCollectionYearLanguage.Clear();
            SeriesCollectionYearLocation.Clear();
            Years.Clear();
            YearsAxes = new string[] { };
            MonthsAxes = new string[] { };

        }
    }
}
    
