using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Services;
using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection.Metadata;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AccommodationBrowserViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        private readonly AccommodationService _accommodationService;
        private int _guestCount;
        public int GuestCount
        {
            get { return _guestCount; }
            set
            {
                if (value != _guestCount)
                {
                    _guestCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _numberOfDays;
        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                if (value != _numberOfDays)
                {
                    _numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _accommodationTypeSelectedIndex;
        public int AccommodationTypeSelectedIndex
        {
            get => _accommodationTypeSelectedIndex;
            set
            {
                if (_accommodationTypeSelectedIndex != value)
                {
                    _accommodationTypeSelectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _sortSelectedIndex;
        public int SortSelectedIndex
        {
            get => _sortSelectedIndex;
            set
            {
                if (_sortSelectedIndex != value)
                {
                    _sortSelectedIndex = value;
                    OnPropertyChanged();
                }
                SortAccommodations();
            }
        }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand GuestCountIncrementCommand { get; }
        public ICommand GuestCountDecrementCommand { get; }
        public ICommand NumberOfDaysIncrementCommand { get; }
        public ICommand NumberOfDaysDecrementCommand { get; }
        public ICommand OpenReservationFormCommand { get; }
        public AccommodationBrowserViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            GuestCount = 1;
            NumberOfDays = 3;
            SortSelectedIndex = 0;
            AccommodationTypeSelectedIndex = 0;
            ApplyFiltersCommand = new ExecuteMethodCommand(ApplyFilters);
            ResetFiltersCommand = new ExecuteMethodCommand(ResetFilters);
            GuestCountIncrementCommand = new IncrementCommand(() => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysIncrementCommand = new IncrementCommand(() => NumberOfDays, (newValue) => NumberOfDays = newValue);
            GuestCountDecrementCommand = new DecrementCommand(this, () => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysDecrementCommand = new DecrementCommand(this, () => NumberOfDays, (newValue) => NumberOfDays = newValue);
            OpenReservationFormCommand = new AccommodationClickCommand(ShowReservationForm);
        }
        private AccommodationType GetSelectedAccommodationType()
        {
            switch (AccommodationTypeSelectedIndex)
            {
                case 0:
                    return AccommodationType.Everything;
                case 1:
                    return AccommodationType.House;
                case 2:
                    return AccommodationType.Apartment;
                default:
                    return AccommodationType.Cottage;
            }
        }
        
        private void ApplyFilters()
        {
            Accommodations.Clear();
            _accommodationService.GetFiltered(SearchText, GetSelectedAccommodationType(), GuestCount, NumberOfDays).
                ForEach(a => Accommodations.Add(a));
        }
        private void ResetFilters()
        {
            SortSelectedIndex = 0;
            SearchText = "";
            GuestCount = 1;
            NumberOfDays = 3;
            AccommodationTypeSelectedIndex = 0;
            Accommodations.Clear();
            _accommodationService.GetAll().ForEach(a => Accommodations.Add(a));
        }
        private void SortAccommodations()
        {
            string criterion = GetSortingCriterion();
            if (criterion == "")
                return;

            var sortedAccommodations = _accommodationService.Sort(Accommodations, criterion);
            Accommodations.Clear();
            sortedAccommodations.ForEach(a => Accommodations.Add(a));
        }

        private string GetSortingCriterion()
        {
            switch (_sortSelectedIndex)
            {
                case 1:
                    return "Name";
                case 2:
                    return "Location";
                case 3:
                    return "MaxGuestCountAsc";
                case 4:
                    return "MaxGuestCountDesc";
                case 5:
                    return "MinDaysNumberAsc";
                case 6:
                    return "MinDaysNumberDesc";
                default:
                    return "";
            }
        }
        private void ShowReservationForm(Accommodation accommodation)
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, _user, accommodation);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
    }
}