using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationBrowser.xaml
    /// </summary>
    public partial class AccommodationBrowser : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        private readonly AccommodationController _controller;
        private int _guestNumber;
        public int GuestNumber
        {
            get { return _guestNumber; }
            set
            {
                if(value != _guestNumber)
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationBrowser(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _controller = new AccommodationController();
            Accommodations = new ObservableCollection<Accommodation>(_controller.GetAll());
            GuestNumber = 1;
            NumberOfDays = 1;

            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        private void ApplyFiltersClick(object sender, RoutedEventArgs e)
        {
            string keyWords = SearchTextBox.Text;
            AccommodationType type = GetType();
            ValidateGuestNumber();
            ValidateNumberOfDays();

            Accommodations.Clear();
            foreach (var accommodation in _controller.GetFiltered(keyWords, type, GuestNumber, NumberOfDays))
            {
                Accommodations.Add(accommodation);
            }
        }
        private AccommodationType GetType()
        {
            switch (TypeComboBox.SelectedIndex)
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
        private void ValidateGuestNumber()
        {
            try
            {
                GuestNumber = int.Parse(GuestNumberTextBox.Text);
            } 
            catch { GuestNumber = 1; }
        }
        private void ValidateNumberOfDays()
        {           
            try
            {
                NumberOfDays = int.Parse(NumberOfDaysTextBox.Text);
            } 
            catch { NumberOfDays = 1; }
        }

        private void ResetFiltersClick(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();
            GuestNumber = 1;
            NumberOfDays = 1;
            TypeComboBox.SelectedIndex = 0;
            Accommodations.Clear();
            foreach (var accommodation in _controller.GetAll())
            {
                Accommodations.Add(accommodation);
            }
        }

        private void AccommodationNameClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedTextBlock = (TextBlock)sender;
            Accommodation accommodation = (Accommodation)clickedTextBlock.DataContext;
            AccommodationReservationWindow accommodationReservation = new AccommodationReservationWindow(LoggedInUser, accommodation);
            accommodationReservation.ShowDialog();
        }

        private void GuestNumberMinusClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(GuestNumberTextBox.Text, out _))
                GuestNumber = 1;
            else
                GuestNumber--;
        }

        private void GuestNumberPlusClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(GuestNumberTextBox.Text, out _))
                GuestNumber = 1;
            else
                GuestNumber++;
        }

        private void NumberOfDaysMinusClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NumberOfDaysTextBox.Text, out _))
                NumberOfDays = 1;
            else
                NumberOfDays--;
        }

        private void NumberOfDaysPlusClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(NumberOfDaysTextBox.Text, out _))
                NumberOfDays = 1;
            else
                NumberOfDays++;
        }

        private void SortByNameClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _controller.SortByName(new List<Accommodation>(Accommodations));
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByMaxGuestNumberClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _controller.SortByMaxGuestNumber(new List<Accommodation>(Accommodations));
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByMinDaysNumberClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _controller.SortByMinDaysNumber(new List<Accommodation>(Accommodations));
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void SortByLocationClick(object sender, RoutedEventArgs e)
        {
            var sortedAccommodations = _controller.SortByLocation(new List<Accommodation>(Accommodations));
            Accommodations.Clear();
            foreach (var accommodation in sortedAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void MyReservationsClick(object sender, RoutedEventArgs e)
        {
            MyAccommodationReservations window= new MyAccommodationReservations(LoggedInUser);
            window.Show();
            Close();
        }
    }
}
