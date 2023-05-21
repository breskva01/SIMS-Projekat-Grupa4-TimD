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
        public ObservableCollection<TourRequest> Requests { get; set; }

        private TourRequestService _tourRequestService;
        private LocationService _locationService;

        public ObservableCollection<Location> Locations { get; set; }

        private SeriesCollection _seriesCollectionYear;
        public SeriesCollection SeriesCollectionYear
        {
            get { return _seriesCollectionYear; }
            set
            {
                _seriesCollectionYear = value;
                OnPropertyChanged(nameof(SeriesCollectionYear));
            }
        }
        private SeriesCollection _seriesCollectionMonth;
        public SeriesCollection SeriesCollectionMonth
        {
            get { return _seriesCollectionMonth; }
            set
            {
                _seriesCollectionMonth = value;
                OnPropertyChanged(nameof(SeriesCollectionMonth));
            }
        }

        public List<int> Years { get; set; }
        public List<string> Months { get; set; }

        public List<int> YearNumberOfRequests { get; set; }

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

        private bool _isCheckedYears;
        public bool IsCheckedYears
        {
            get { return _isCheckedYears; }
            set
            {
                 _isCheckedYears = value;
                if (_isCheckedYears == true)
                {
                    OnPropertyChanged(nameof(IsCheckedYears));
                    InitializeYearsGraph();
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
                if (_isCheckedMonths == true)
                {
                    OnPropertyChanged(nameof(IsCheckedMonths));
                    PopulateYearsComboBox();
                }
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedCity));
                InitializeMonthsGraph();
            }
        }
        private List<int> _allYears;
        public List<int> AllYears
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

        private readonly NavigationStore _navigationStore;
        private User _user;

        public Func<double, string> YFormatter { get; }

        public ICommand CreateCommand { get; set; }

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

            YearsAxes = new string[] { };
            MonthsAxes = new string[] { };
            Months = new List<string>();

            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();

            _seriesCollectionYear = new SeriesCollection();
            _seriesCollectionMonth = new SeriesCollection();

            AllYears = new List<int>();
            /*
            SelectedCity = "Beograd";
            SelectedCountry = "Srbija";
            InitializeYearsGraph();
            */

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
            SeriesCollectionYear.Clear();
            AllYears = _tourRequestService.GetAllYears();
        }
        private void InitializeYearsGraph()
        {
            // SeriesCollectionYear.Clear();
            //SeriesCollectionYear = new SeriesCollection();
            SeriesCollectionMonth.Clear();

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
            // za izabranu lokaciju treba da nadjem sve godine requestova

            LineSeries YearNumberOfRequestAxes = new LineSeries();
            YearNumberOfRequestAxes.Title = "yearRequests";
            YearNumberOfRequestAxes.Values = new ChartValues<int>(YearNumberOfRequests);
            YearNumberOfRequestAxes.LineSmoothness = 0;
            SeriesCollectionYear = new SeriesCollection();
            SeriesCollectionYear.Add(YearNumberOfRequestAxes);
        }
        private void InitializeMonthsGraph()
        {
            SeriesCollectionYear.Clear();
            List<TourRequest> allTourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in allTourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }

            List<TourRequest> requests = _tourRequestService.GetForChosenLocation(allTourRequests, SelectedCountry, SelectedCity);

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

            MonthsAxes = new string[Months.Count];
            List<int> MonthsNumberOfRequests = new List<int>();

            for (int j = 1; j <= Months.Count(); j++)
            {
                MonthsNumberOfRequests.Add(_tourRequestService.GetMonthNumberOfRequests(j, SelectedCountry, SelectedCity,SelectedYear, requests)); 
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
            SeriesCollectionMonth = new SeriesCollection();
            SeriesCollectionMonth.Add(MonthNumberOfRequestAxes);

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
    }
}
    
