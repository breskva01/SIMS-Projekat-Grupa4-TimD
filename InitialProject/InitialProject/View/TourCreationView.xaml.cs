using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourCreationView.xaml
    /// </summary>
    public partial class TourCreationView : Window, INotifyPropertyChanged
    {
        private List<Location> locations;
        private readonly Storage<Location> _storage;
        private const string FilePath = "../../../Resources/Data/locations.csv";
        private readonly TourController _tourController;
        

        private string _tourName;
        public new string TourName
        {
            get => _tourName;
            set
            {
                if(value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
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
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _languageType;
        public string LanguageType
        {
            get => _languageType;
            set
            {
                if (value != _languageType)
                {
                    _languageType = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _pictureUrl;
        public string PictureUrl
        {
            get => _pictureUrl;
            set
            {
                if (value != _pictureUrl)
                {
                    _pictureUrl = value;
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
        private string _town;
        public string Town
        {
            get => _town;
            set
            {
                if (value != _town)
                {
                    _town = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCreationView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _storage = new Storage<Location>(FilePath);
            locations = _storage.Load();

            // Set the items source of the country combo box to the distinct list of countries.
            countryComboBox.ItemsSource = locations.Select(l => l.Country).Distinct();

            _tourController = new TourController();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void TourCreationClick(object sender, RoutedEventArgs e)
        {
            Location Location = new Location();
            Location.Id = locations.Where(l => l.City == Town).Select(l => l.Id).FirstOrDefault();
            Location.Country = Country;
            Location.City = Town;
           
            int TourDuration = int.Parse(Duration);
            int MaxGuests = int.Parse(MaximumGuests);
            GuideLanguage lang = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), LanguageType);
            _tourController.CreateTour(TourName, Location, Description, lang, MaxGuests, Start, TourDuration, PictureUrl);
            Close();

        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var currentText = (sender as ComboBox).Text;

            // Filter the items based on the current text
            //var filteredItems = Locations.Where(l => l.StartsWith(currentText, StringComparison.OrdinalIgnoreCase)).ToList();

            // Set the ItemsSource of the combobox to the filtered items
            //(sender as ComboBox).ItemsSource = filteredItems;
        }

        private void countryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected country.
            string selectedCountry = (string)countryComboBox.SelectedValue;

            // Set the items source of the city combo box to the cities of the selected country.
            cityComboBox.ItemsSource = locations.Where(l => l.Country == selectedCountry);
        }
    }
}
