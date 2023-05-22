using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourSortViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        private TourFilterSort _tourFilterSort;
        private readonly TourService _tourService;

        private bool _isSortCountryChecked;
        public bool IsSortCountryChecked
        {
            get { return _isSortCountryChecked; }
            set
            {
                _isSortCountryChecked = value;
                OnPropertyChanged(nameof(IsSortCountryChecked));

            }
        }

        private bool _isSortCityChecked;
        public bool IsSortCityChecked
        {
            get { return _isSortCityChecked; }
            set
            {
                _isSortCityChecked = value;
                OnPropertyChanged(nameof(IsSortCityChecked));

            }
        }

        private bool _isSortDurationChecked;
        public bool IsSortDurationChecked
        {
            get { return _isSortDurationChecked; }
            set
            {
                _isSortDurationChecked = value;
                OnPropertyChanged(nameof(IsSortDurationChecked));

            }
        }

        private bool _isSortLanguageChecked;
        public bool IsSortLanguageChecked
        {
            get { return _isSortLanguageChecked; }
            set
            {
                _isSortLanguageChecked = value;
                OnPropertyChanged(nameof(IsSortLanguageChecked));

            }
        }

        private bool _isSortEmptySpacesChecked;
        public bool IsSortEmptySpacesChecked
        {
            get { return _isSortEmptySpacesChecked; }
            set
            {
                _isSortEmptySpacesChecked = value;
                OnPropertyChanged(nameof(IsSortEmptySpacesChecked));

            }
        }

        public ICommand SortCommand { get; }
        public ICommand BackCommand { get; }

        public TourSortViewModel(NavigationStore navigationStore, User user, TourFilterSort tourFilterSort)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _tourFilterSort = tourFilterSort;

            _isSortCountryChecked = tourFilterSort.SortCountry;
            _isSortCityChecked = tourFilterSort.SortCity;
            _isSortDurationChecked = tourFilterSort.SortDuration;
            _isSortLanguageChecked = tourFilterSort.SortLanguage;
            _isSortEmptySpacesChecked = tourFilterSort.SortSpaces;

            
            

            SortCommand = new ExecuteMethodCommand(PassSorts);
            BackCommand = new ExecuteMethodCommand(ShowTourBrowserView);
            
        }

        private void PassSorts()
        {
            _tourFilterSort.SortCountry = IsSortCountryChecked;
            _tourFilterSort.SortCity = IsSortCityChecked;
            _tourFilterSort.SortDuration = IsSortDurationChecked;
            _tourFilterSort.SortLanguage = IsSortLanguageChecked;
            _tourFilterSort.SortSpaces = IsSortEmptySpacesChecked;

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
    }
}
