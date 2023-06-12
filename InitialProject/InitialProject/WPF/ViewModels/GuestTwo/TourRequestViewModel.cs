using CommunityToolkit.Mvvm.Input;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourRequestViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly LocationService _locationService;
        private readonly TourRequestService _tourRequestService;
        private User _user;

        public bool isEarliestDateSelected { get; set; }
        public bool isLatestDateSelected { get; set; }

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
                CheckIsEverythingComplete();
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
                CheckIsEverythingComplete();
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
            Cities.Sort();
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

        private int _selectedNumberOfGuests;
        public int SelectedNumberOfGuests
        {
            get { return _selectedNumberOfGuests; }
            set
            {
                if (value >= 0)
                {
                    _selectedNumberOfGuests = value;
                    OnPropertyChanged(nameof(SelectedNumberOfGuests));
                }

            }
        }

        private DateTime _selectedEarliestDate;
        public DateTime SelectedEarliestDate
        {
            get => _selectedEarliestDate;
            set
            {
                if (value != _selectedEarliestDate)
                {
                    isEarliestDateSelected = true;
                    _selectedEarliestDate = value;
                    CheckIsEverythingComplete();
                    IsEnabledLatestDate = true;
                    OnPropertyChanged(nameof(IsEnabledLatestDate));
                    OnPropertyChanged();
                    if (SelectedLatestDate < SelectedEarliestDate)
                    {
                        SelectedLatestDate = SelectedEarliestDate;
                        OnPropertyChanged(nameof(SelectedLatestDate));
                    }
                }
            }
        }

        private DateTime _selectedLatestDate;
        public DateTime SelectedLatestDate
        {
            get => _selectedLatestDate;
            set
            {
                if (value != _selectedLatestDate)
                {
                    isLatestDateSelected = true;
                    CheckIsEverythingComplete();
                    _selectedLatestDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEnabledLatestDate;
        public bool IsEnabledLatestDate
        {
            get { return _isEnabledLatestDate; }
            set
            {
                _isEnabledLatestDate = value;
                OnPropertyChanged(nameof(IsEnabledLatestDate));
            }
        }

        private bool _isEverythingComplete;
        public bool IsEverythingComplete
        {
            get { return _isEverythingComplete; }
            set
            {
                _isEverythingComplete = value;
                OnPropertyChanged(nameof(IsEverythingComplete));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand IncreaseGuestsCommand => new RelayCommand(() => SelectedNumberOfGuests++);
        public ICommand DecreaseGuestsCommand => new RelayCommand(() => SelectedNumberOfGuests--);
        public ICommand RequestCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }
        public TourRequestViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _locationService = new LocationService();
            _tourRequestService = new TourRequestService();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();
            Countries.Sort();

            SelectedCountry = string.Empty;
            SelectedCity = string.Empty;
            SelectedLanguageIndex = 0;
            SelectedNumberOfGuests = 0;
            SelectedEarliestDate = DateTime.Now;
            SelectedLatestDate = DateTime.Now;
            Description = string.Empty;
            IsEnabledLatestDate = false;
            IsEverythingComplete = false;
            isEarliestDateSelected = false;
            isLatestDateSelected = false;

            RequestCommand = new ExecuteMethodCommand(CreateTourRequest);
            BackCommand = new ExecuteMethodCommand(ShowGuest2RequestMenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
        }

        public void CheckIsEverythingComplete()
        {
            IsEverythingComplete = (SelectedCountry != string.Empty && SelectedCity != string.Empty) && (isEarliestDateSelected && isLatestDateSelected);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowGuest2RequestMenuView()
        {
            Guest2RequestMenuViewModel guest2RequestMenuViewModel = new Guest2RequestMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2RequestMenuViewModel));
            navigate.Execute(null);
        }

        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }

        public void CreateTourRequest()
        {
            Location Location = new Location();
            Location.Country = SelectedCountry;
            Location.City = SelectedCity;
            Location.Id = Locations.Where(c => c.City == SelectedCity).Select(c => c.Id).FirstOrDefault();
            GuideLanguage Language = GetLanguage();

            _tourRequestService.CreateTourRequest(_user.Id, Location, Description, RequestStatus.OnHold, Language, SelectedNumberOfGuests, SelectedEarliestDate, SelectedLatestDate, 0);
            ShowGuest2RequestMenuView();
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

        
    }
}
