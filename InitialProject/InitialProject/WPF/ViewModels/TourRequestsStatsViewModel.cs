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

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsStatsViewModel : ViewModelBase
    {
        public ObservableCollection<TourRequest> Requests { get; set; }

        private TourRequestService _tourRequestService;
        private LocationService _locationService;

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

        public ICommand CreateCommand { get; set; }

        public TourRequestsStatsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            Requests = new ObservableCollection<TourRequest>(_tourRequestService.GetAll());

            foreach (TourRequest tourRequest in Requests)
            {
                tourRequest.Location = _locationService.GetById(tourRequest.Location.Id);
            }
            CreateCommand = new ExecuteMethodCommand(CreateTour);

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
    
