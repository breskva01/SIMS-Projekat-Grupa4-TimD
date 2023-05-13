﻿using InitialProject.Domain.Models;
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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class AccommodationBrowserViewModel : ViewModelBase
    {
        private readonly User _user;
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
        public AccommodationType[] AccommodationTypes { get; } = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>().ToArray();

        private AccommodationType _selectedAccommodationType;
        public AccommodationType SelectedAccommodationType
        {
            get => _selectedAccommodationType;
            set
            {
                if (_selectedAccommodationType != value)
                {
                    _selectedAccommodationType = value;
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
        public AccommodationBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            GuestCount = 1;
            NumberOfDays = 1;
            SortSelectedIndex = 0;
            SelectedAccommodationType = AccommodationType.Everything;
            ApplyFiltersCommand = new ExecuteMethodCommand(ApplyFilters);
            ResetFiltersCommand = new ExecuteMethodCommand(ResetFilters);
            GuestCountIncrementCommand = new IncrementCommand(() => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysIncrementCommand = new IncrementCommand(() => NumberOfDays, (newValue) => NumberOfDays = newValue);
            GuestCountDecrementCommand = new DecrementCommand(this, () => GuestCount, (newValue) => GuestCount = newValue);
            NumberOfDaysDecrementCommand = new DecrementCommand(this, () => NumberOfDays, (newValue) => NumberOfDays = newValue);
            OpenReservationFormCommand = new AccommodationClickCommand(ShowReservationForm);
        }
        
        private void ApplyFilters()
        {
            Accommodations.Clear();
            _accommodationService.GetFiltered(SearchText, SelectedAccommodationType, GuestCount, NumberOfDays).
                ForEach(a => Accommodations.Add(a));
        }
        private void ResetFilters()
        {
            SortSelectedIndex = 0;
            SearchText = "";
            GuestCount = NumberOfDays = 1;
            SelectedAccommodationType = AccommodationType.Everything;
            Accommodations.Clear();
            _accommodationService.GetAll().ForEach(a => Accommodations.Add(a));
        }
        private void SortAccommodations()
        {
            string criterion = GetSortingCriterion();
            if (criterion == "")
                return;

            List<Accommodation> sortedAccommodations;      
            sortedAccommodations = _accommodationService.Sort(Accommodations, criterion);
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