using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
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
        private List<City> cities;
        private readonly Storage<City> citiesStorage;
        private const string FilePath = "../../../Resources/Data/cities.csv";


        public ToursView(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _controller = new TourController();
            Tours = new ObservableCollection<Tour>(_controller.GetAll());
            citiesStorage = new Storage<City>(FilePath);
            cities = citiesStorage.Load();
            cmbCountry.ItemsSource = cities.Select(c => c.Country).Distinct();


            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        private void cmbCountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCountry = (string)cmbCountry.SelectedValue;
            cmbCity.ItemsSource = cities.Where(c => c.Country == selectedCountry);
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            string country = cmbCountry.Text;
            string city = cmbCity.Text;
            int duration = GetDuration();
            GuideLanguage language = GetLanguage();
            int currentNumberOfGuests = GetCurrentNumberOfGuests();

            Tours.Clear();
            foreach (var tour in _controller.GetFiltered(country, city, duration, language, currentNumberOfGuests))
            {
                Tours.Add(tour);
            }
        }

        private int GetDuration()
        {
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

        private int GetCurrentNumberOfGuests()
        {
            int currentNumberOfGuests = 0;
            try
            {
                currentNumberOfGuests = int.Parse(tbNumberOfGuests.Text);
            }
            catch { };

            return currentNumberOfGuests;
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
                Tours.Add(tour);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        
    }
}
