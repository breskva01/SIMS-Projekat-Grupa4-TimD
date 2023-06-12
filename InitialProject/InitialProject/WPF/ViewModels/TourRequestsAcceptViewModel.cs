using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsAcceptViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private ObservableCollection<TourRequest> _requests;
        public ObservableCollection<TourRequest> TourRequests 
        {
            get { return _requests; } 
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(TourRequests));
            }
        }

        private TourRequestService _tourRequestService;
        private LocationService _locationService;

        public ObservableCollection<Location> Locations { get; set; }

        public TourRequest SelectedTourRequest { get; set; }

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

        private string _earliestDate;
        public string EarliestDate
        {
            get => _earliestDate;
            set
            {
                if (value != _earliestDate)
                {
                    _earliestDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _latestDate;
        public string LatestDate
        {
            get => _latestDate;
            set
            {
                if (value != _latestDate)
                {
                    _latestDate = value;
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
        private int _numberOfGuests;
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests && value >= 0)
                {
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand FilterCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand IncreaseGuestsCommand { get; set; }
        public ICommand DecreaseGuestsCommand { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ICommand CreateTourCommand { get; set; }
        public ICommand LiveTrackingCommand { get; set; }
        public ICommand CancelTourCommand { get; set; }
        public ICommand TourStatsCommand { get; set; }
        public ICommand RatingsViewCommand { get; set; }
        public ICommand TourRequestsCommand { get; set; }
        public ICommand TourRequestsStatsCommand { get; set; }
        public ICommand GuideProfileCommand { get; set; }
        public ICommand ComplexTourCommand { get; set; }
        public ICommand StatsCommand { get; set; }



        public TourRequestsAcceptViewModel(NavigationStore navigationStore, User user) 
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            SelectedTourRequest = new TourRequest();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());
            
            foreach( TourRequest tourRequest in TourRequests)
            {
                tourRequest.Location = _locationService.GetById(tourRequest.Location.Id);
            }

            Countries = Locations.Select(l => l.Country).Distinct().ToList();

            FilterCommand = new ExecuteMethodCommand(ApplyFilter);
            ResetCommand = new ExecuteMethodCommand(ResetFilter);
            IncreaseGuestsCommand = new ExecuteMethodCommand(IncreaseGuests);
            DecreaseGuestsCommand = new ExecuteMethodCommand(DecreaseGuests);
            AcceptCommand = new ExecuteMethodCommand(AcceptTourRequest);
            CreateTourCommand = new ExecuteMethodCommand(ShowTourCreationView);
            LiveTrackingCommand = new ExecuteMethodCommand(ShowToursTodayView);
            CancelTourCommand = new ExecuteMethodCommand(ShowTourCancellationView);
            TourStatsCommand = new ExecuteMethodCommand(ShowTourStatsView);
            RatingsViewCommand = new ExecuteMethodCommand(ShowGuideRatingsView);
            TourRequestsCommand = new ExecuteMethodCommand(ShowTourRequestsView);
            TourRequestsStatsCommand = new ExecuteMethodCommand(ShowTourRequestsStatsView);
            StatsCommand = new ExecuteMethodCommand(ShowTourRequestsStatsView);
            GuideProfileCommand = new ExecuteMethodCommand(ShowGuideProfileView);
            ComplexTourCommand = new ExecuteMethodCommand(ShowComplexTourView);
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
        private void ApplyFilter()
        {
            string country = SelectedCountry;
            string city = SelectedCity;
            DateTime earliestDate = new DateTime();
            DateTime latestDate = new DateTime();
            /*
            GuideLanguage language = new GuideLanguage();
            if (Language != "")
                language = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), Language);
            else
                language = ;
            */
            if (EarliestDate != "")
                earliestDate = Convert.ToDateTime(EarliestDate);

            if (LatestDate != "")
                latestDate = Convert.ToDateTime(LatestDate);

            int numberOfGuests = NumberOfGuests;

            TourRequests.Clear();
            foreach (var tourRequest in _tourRequestService.GetFiltered(country, city, earliestDate, latestDate, numberOfGuests, Language))
            {
                tourRequest.Location = Locations.FirstOrDefault(l => l.Id == tourRequest.Location.Id);
                TourRequests.Add(tourRequest);
            }

        }
        private void ResetFilter()
        {
            SelectedCity = null;
            SelectedCountry = null;
            Language = null;
            EarliestDate = null;
            LatestDate = null;
            NumberOfGuests = 0;
            TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());

            foreach (TourRequest tourRequest in TourRequests)
            {
                tourRequest.Location = _locationService.GetById(tourRequest.Location.Id);
            }
        }
        private void IncreaseGuests()
        {
            NumberOfGuests++;
        }
        private void DecreaseGuests()
        {
            NumberOfGuests--;
        }
        private void AcceptTourRequest()
        {
           
            if(SelectedTourRequest != null)
            {
                SelectedTourRequest.Status = RequestStatus.Approved;
                TourRequestsAcceptDatePickerView view = new TourRequestsAcceptDatePickerView(_navigationStore, _user, SelectedTourRequest);
                view.Show();
                return;
            }
            return;
        }
        private void ShowTourCreationView()
        {
            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }

        private void ShowComplexTourView()
        {
            ComplexTourAcceptViewModel viewModel = new ComplexTourAcceptViewModel(_navigationStore, _user);
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
        private void ShowTourRequestsView()
        {
            TourRequestsAcceptViewModel viewModel = new TourRequestsAcceptViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowTourRequestsStatsView()
        {
            TourRequestsStatsViewModel viewModel = new TourRequestsStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
        private void ShowGuideProfileView()
        {
            GuideProfileViewModel viewModel = new GuideProfileViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, viewModel));

            navigate.Execute(null);
        }
    }
}
