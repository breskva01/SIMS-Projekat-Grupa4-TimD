using CommunityToolkit.Mvvm.Input;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourFilterViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly LocationService _locationService;
        private TourFilterSort _tourFilterSort;
        private User _user;

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

        private int _selectedMinDuration;
        public int SelectedMinDuration
        {
            get { return _selectedMinDuration; }
            set
            {
                if (value >= 0)
                {
                    _selectedMinDuration = value;
                    if (_selectedMaxDuration < _selectedMinDuration)
                    {
                        SelectedMaxDuration = _selectedMinDuration;
                        OnPropertyChanged(nameof(SelectedMaxDuration));
                    }
                    OnPropertyChanged(nameof(SelectedMinDuration));
                }

            }
        }

        private int _selectedMaxDuration;
        public int SelectedMaxDuration
        {
            get { return _selectedMaxDuration; }
            set
            {
                _selectedMaxDuration = value;
                OnPropertyChanged(nameof(SelectedMaxDuration));

                if(_selectedMaxDuration < SelectedMinDuration)
                {
                    SelectedMaxDuration = _selectedMinDuration;
                    OnPropertyChanged(nameof(SelectedMaxDuration));
                }
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

        private int _selectedNumberOfGuests;
        public int SelectedNumberOfGuests
        {
            get { return _selectedNumberOfGuests; }
            set
            {
                if(value >= 0)
                {
                    _selectedNumberOfGuests = value;
                    OnPropertyChanged(nameof(SelectedNumberOfGuests));
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public ICommand FilterCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand IncreaseMinCommand => new RelayCommand(() => SelectedMinDuration++);
        public ICommand DecreaseMinCommand => new RelayCommand(() => SelectedMinDuration--);
        public ICommand IncreaseMaxCommand => new RelayCommand(() => SelectedMaxDuration++);
        public ICommand DecreaseMaxCommand => new RelayCommand(() => SelectedMaxDuration--);
        public ICommand IncreaseGuestsCommand => new RelayCommand(() => SelectedNumberOfGuests++);
        public ICommand DecreaseGuestsCommand => new RelayCommand(() => SelectedNumberOfGuests--);
        public TourFilterViewModel(NavigationStore navigationStore, User user, TourFilterSort tourFilterSort)
        {
            _navigationStore = navigationStore;
            _user = user;
            _locationService = new LocationService();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Countries = Locations.Select(l => l.Country).Distinct().ToList();

            _tourFilterSort = tourFilterSort;
            SelectedCountry = tourFilterSort.FilterCountry;
            SelectedCity = tourFilterSort.FilterCity;
            SelectedLanguageIndex = Convert.ToInt32(tourFilterSort.FilterLanguage);
            SelectedMinDuration = tourFilterSort.FilterMinDuration;
            SelectedMaxDuration = tourFilterSort.FilterMaxDuration;
            SelectedNumberOfGuests = tourFilterSort.FilterNumberOfGuests;
            
            
            

            FilterCommand = new ExecuteMethodCommand(PassFilters);
            BackCommand = new ExecuteMethodCommand(ShowTourBrowserView);
            _tourFilterSort = tourFilterSort;
        }

        private void PassFilters()
        {
            _tourFilterSort.FilterCountry = SelectedCountry;
            _tourFilterSort.FilterCity = SelectedCity;
            _tourFilterSort.FilterMinDuration = SelectedMinDuration;
            _tourFilterSort.FilterMaxDuration = SelectedMaxDuration;
            _tourFilterSort.FilterLanguage = GetLanguage();
            _tourFilterSort.FilterNumberOfGuests = SelectedNumberOfGuests;

            NewTourBrowserViewModel tourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user, _tourFilterSort);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourBrowserViewModel));
            navigate.Execute(null);
        }

        private void ShowTourBrowserView()
        {
            NewTourBrowserViewModel tourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourBrowserViewModel));
            navigate.Execute(null);
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
