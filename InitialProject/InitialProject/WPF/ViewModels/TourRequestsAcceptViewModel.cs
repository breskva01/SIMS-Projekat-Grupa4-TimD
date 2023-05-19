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
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsAcceptViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public ObservableCollection<TourRequest> TourRequests { get; set; }

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
            SelectedTourRequest.Status = RequestStatus.Accepted;
            _tourRequestService.Update(SelectedTourRequest);
        }
    }
}
