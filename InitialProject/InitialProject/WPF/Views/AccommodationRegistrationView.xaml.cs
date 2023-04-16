using InitialProject.Application.Storage;
using InitialProject.Controller;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Window, INotifyPropertyChanged
    {

        private List<Location> _locations;
        private readonly Storage<Location> _storage;
        private readonly AccommodationController _controller;
        private readonly User _owner;
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (value != _address)
                {
                    _address= value;
                }
            }
        }

        private AccommodationType _type;
        public AccommodationType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maximumGuests;
        public string MaximumGuests
        {
            get => _maximumGuests;
            set
            {
                if (value != _maximumGuests)
                {
                    _maximumGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _minimumDays;
        public string MinimumDays
        {
            get => _minimumDays;
            set
            {
                if (value != _minimumDays)
                {
                    _minimumDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _minimumCancellationNotice = "1";
        public string MinimumCancellationNotice
        {
            get => _minimumCancellationNotice;
            set
            {
                if (value != _minimumCancellationNotice)
                {
                    _minimumCancellationNotice = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _pictureURL;
        public string PictureURL
        {
            get => _pictureURL;
            set
            {
                if (value != _pictureURL)
    {
                    _pictureURL = value;
                    OnPropertyChanged();
                }
            }
        }


        public AccommodationRegistrationView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _controller= new AccommodationController();
            // Assume locationsList is already initialized with data.
            _storage = new Storage<Location>(FilePath);
            _locations = _storage.Load();
            _owner= user;

            // Set the items source of the country combo box to the distinct list of countries.
            countryComboBox.ItemsSource = _locations.Select(c => c.Country).Distinct();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected country.
            string selectedCountry = (string)countryComboBox.SelectedValue;

            // Set the items source of the city combo box to the cities of the selected country.
            cityComboBox.ItemsSource = _locations.Where(c => c.Country == selectedCountry);
        }

        private void RegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            int maximumGuests = int.Parse(MaximumGuests);
            int minimumDays = int.Parse(MinimumDays);
            int minimumCancellationNotice = int.Parse(MinimumCancellationNotice);
            int LocationId = GetLocationId();
            if (LocationId == -1)
            {
                MessageBox.Show("Nije uneta lokacija");
                Close();
            }
            _controller.RegisterAccommodation(AccommodationName, Country, City , Address, Type, maximumGuests, minimumDays, minimumCancellationNotice, PictureURL, _owner, _owner.Id);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int GetLocationId()
        {
            int LocationId;
            foreach (Location l in _locations)
            {
                if (City == l.City && Country == l.Country)
                    return LocationId = l.Id;
            }
            return -1;
        }
    }
}
