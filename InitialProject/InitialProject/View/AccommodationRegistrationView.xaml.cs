using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Window, INotifyPropertyChanged
    {

        private List<Location> cities;
        private readonly Storage<Location> _storage;
        private const string FilePath = "../../../Resources/Data/locations.csv";

        

        public AccommodationRegistrationView(User user)
        {
            InitializeComponent();

            // Assume citiesList is already initialized with data.
            _storage = new Storage<Location>(FilePath);
            cities = _storage.Load();

            // Set the items source of the country combo box to the distinct list of countries.
            countryComboBox.ItemsSource = cities.Select(c => c.Country).Distinct();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected country.
            string selectedCountry = (string)countryComboBox.SelectedValue;

            // Set the items source of the city combo box to the cities of the selected country.
            cityComboBox.ItemsSource = cities.Where(c => c.Country == selectedCountry);
        }

        private void RegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
