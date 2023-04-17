using InitialProject.Application.Storage;
using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for ToursView.xaml
    /// </summary>
    public partial class ToursView : Window, INotifyPropertyChanged
    {
        public User LoggedInUser { get; set; }

        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        private readonly TourController _controller;

        private List<Location> _locations;
        private readonly Storage<Location> _locationStorage;
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private Regex _notNumber = new Regex(@"\D+");


        public ToursView(User user)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = user;

            _controller = new TourController();
            Tours = new ObservableCollection<Tour>(_controller.GetAll());

            _locationStorage = new Storage<Location>(FilePath);
            _locations = _locationStorage.Load();

            foreach (Tour t in Tours)
            {
                t.Location = _locations.FirstOrDefault(l => l.Id == t.LocationId);
            }

            cmbCountry.ItemsSource = _locations.Select(l => l.Country).Distinct();


            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        private void cmbCountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCountry = (string)cmbCountry.SelectedValue;
            cmbCity.ItemsSource = _locations.Where(l => l.Country == selectedCountry);
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            string country = cmbCountry.Text;
            string city = cmbCity.Text;
            int duration = GetDuration();
            GuideLanguage language = GetLanguage();
            int NumberOfGuests = GetNumberOfGuests();

            Tours.Clear();
            foreach (var tour in _controller.GetFiltered(country, city, duration, language, NumberOfGuests))
            {
                tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }
        }

        private int GetDuration()
        {
            Match match = _notNumber.Match(tbDuration.Text);
            if (match.Success)
            {
              MessageBox.Show("You cannot enter non-number character in the Duration box.");
            }

            int duration = 0;
            try
            {
                duration = int.Parse(tbDuration.Text);
                
            }
            catch { };
            
            return duration;
        }

        private GuideLanguage GetLanguage()
        {
            switch (cmbLanguage.SelectedIndex)
            {
                case 0:
                    return GuideLanguage.All;
                case 1:
                    return GuideLanguage.Serbian;
                default:
                    return GuideLanguage.English;
            }
        }

        private int GetNumberOfGuests()
        {
            Match match = _notNumber.Match(tbNumberOfGuests.Text);
            if (match.Success)
            {
                MessageBox.Show("You cannot enter non-number character in the Number of guests box.");
            }
            int NumberOfGuests = 0;

            try
            {
                NumberOfGuests = int.Parse(tbNumberOfGuests.Text);
            }
            catch { };

            return NumberOfGuests;
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            cmbCountry.SelectedItem = ""; 
            cmbCity.SelectedItem = "";
            tbDuration.Clear();
            cmbLanguage.SelectedIndex = 0;
            tbNumberOfGuests.Clear();

            Tours.Clear();
            foreach (var tour in _controller.GetAll())
            {
                tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }

        }

        private void ReserveSpotsClick(object sender, RoutedEventArgs e)
        {
            SelectedTour = (Tour)lbTours.SelectedItem;
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");
                return;   
            }
            if (SelectedTour.CurrentNumberOfGuests == SelectedTour.MaximumGuests)
            {
                MessageBox.Show("Unfortunately, the tour that you are interested in is fully booked. On the previous window you can take a look at other tours that are located in the same location.");
                
                Tours.Clear();
                foreach (var tour in _controller.GetFiltered(SelectedTour.Location.Country, SelectedTour.Location.City, 0, GuideLanguage.All, 1))
                {
                    tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                    Tours.Add(tour);
                }

                return;
            }

            TourReservationWindow tourReservationWindow = new TourReservationWindow(SelectedTour, LoggedInUser);
            tourReservationWindow.ShowDialog();

            Tours.Clear();
            foreach (var tour in _controller.GetAll())
            {
                tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SortClick(object sender, RoutedEventArgs e)
        {
            spTourSort.Visibility = Visibility.Visible;
        }

        private void ApplySortClick(object sender, RoutedEventArgs e)
        {
            if (sortByNameCheckBox.IsChecked == true)
            {
                var sortedTours = _controller.SortByName(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (sortByLocationCheckBox.IsChecked == true)
            {
                // Call the sort by date method
                var sortedTours = _controller.SortByLocation(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (sortByDurationCheckBox.IsChecked == true)
            {
                // Call the sort by size method
                var sortedTours = _controller.SortByDuration(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            else if (sortByLanguageCheckBox.IsChecked == true)
            {
                // Call the sort by size method
                var sortedTours = _controller.SortByLanguage(new List<Tour>(Tours));
                Tours.Clear();
                foreach (var tour in sortedTours)
                {
                    Tours.Add(tour);
                    tour.Location = _locations.FirstOrDefault(l => l.Id == tour.LocationId);
                }
            }
            // Hide the sortOptionsPanel after sorting is applied
            spTourSort.Visibility = Visibility.Collapsed;
        }

        
    }
}
