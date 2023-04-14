using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
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

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationBrowserViewModel : ViewModelBase
    {
        private readonly User LoggedInUser;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        private readonly AccommodationService _service;
        private int _guestNumber;
        public int GuestNumber
        {
            get { return _guestNumber; }
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
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
        public int TypeSelectedIndex { get; set; }
        public AccommodationBrowserViewModel(User user)
        {
            LoggedInUser = user;
            _service = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_service.GetAll());
            GuestNumber = 1;
            NumberOfDays = 1;
        }

        private void ApplyFiltersClick(object sender, RoutedEventArgs e)
        {
            AccommodationType type = GetType();

            Accommodations.Clear();
            foreach (var accommodation in _service.GetFiltered(SearchText, type, GuestNumber, NumberOfDays))
            {
                Accommodations.Add(accommodation);
            }
        }
        private AccommodationType GetType()
        {
            switch (TypeSelectedIndex)
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
        private void ResetFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchText = "";
            GuestNumber = 1;
            NumberOfDays = 1;
            TypeSelectedIndex = 0;
            Accommodations.Clear();
            foreach (var accommodation in _service.GetAll())
            {
                Accommodations.Add(accommodation);
            }
        }

        private void AccommodationNameClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = (TextBlock)sender;
            Accommodation accommodation = (Accommodation)clickedTextBlock.DataContext;
            var window = new AccommodationReservationView(LoggedInUser, accommodation);
            window.ShowDialog();
        }

        private void GuestNumberMinusClick(object sender, RoutedEventArgs e)
        {
            if (GuestNumber <= 1)
                GuestNumber = 1;
            else
                GuestNumber--;
        }

        private void GuestNumberPlusClick(object sender, RoutedEventArgs e)
        {
            GuestNumber++;
        }

        private void NumberOfDaysMinusClick(object sender, RoutedEventArgs e)
        {
            if (NumberOfDays <= 1)
                NumberOfDays = 1;
            else
                NumberOfDays--;
        }

        private void NumberOfDaysPlusClick(object sender, RoutedEventArgs e)
        {
            NumberOfDays++;
        }

        private void SortByNameClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), "Name");
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByMaxGuestNumberClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), "MaxGuestNumber");
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByMinDaysNumberClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), "MinDaysNumber");
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByLocationClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _service.Sort(new List<Accommodation>(Accommodations), "Location");
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void MyReservationsClick(object sender, RoutedEventArgs e)
        {
            var window = new MyAccommodationReservationsView(LoggedInUser);
            window.Show();
        }
    }
}
