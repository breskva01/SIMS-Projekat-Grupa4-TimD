using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
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
        private List<Location> _locations;
        private List<KeyPoint> _keyPoints;

        private readonly Storage<Location> _storageLocation;
        private readonly Storage<KeyPoint> _storageKeyPoint;

        private const string FilePath = "../../../Resources/Data/locations.csv";
        private const string FilePathKeyPoint = "../../../Resources/Data/keyPoints.csv";

        private readonly TourController _tourController;

        private List<KeyPoint> _tourKeyPoints = new List<KeyPoint>();
        private List<int> _keyPointIds = new List<int>();

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

            _storageLocation= new Storage<Location>(FilePath);
            _storageKeyPoint = new Storage<KeyPoint>(FilePathKeyPoint);
            _keyPoints = _storageKeyPoint.Load();
            _locations = _storageLocation.Load();
         

            // Set the items source of the country combo box to the distinct list of countries.
            countryComboBox.ItemsSource = _locations.Select(c => c.Country).Distinct();

            keyPointCity.ItemsSource = _locations.Select(c => c.City).Distinct();

            _tourController = new TourController();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void TourCreationClick(object sender, RoutedEventArgs e)
        {
            if(_tourKeyPoints.Count() > 1)
            {
                Location Location = new Location();
                Location.Country = Country;
                Location.City = Town;
                Location.Id = _locations.Where(c => c.City == Town).Select(c => c.Id).FirstOrDefault();

                int TourDuration = int.Parse(Duration);
                int MaxGuests = int.Parse(MaximumGuests);

                GuideLanguage lang = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), LanguageType);

                foreach (KeyPoint ky in _tourKeyPoints)
                {
                    _keyPointIds.Add(ky.Id);
                }
                Start = (DateTime)dateTimePicker.Value;

                _tourController.CreateTour(TourName, Location, Description, lang, MaxGuests, Start, TourDuration, PictureUrl, _tourKeyPoints, _keyPointIds);
                MessageBox.Show(" Tour successfully created! ");
            }
            else
            {
                MessageBox.Show(" There must be atleast two keypoints! ");
            }

        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void countryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected country.
            string selectedCountry = (string)countryComboBox.SelectedValue;

            // Set the items source of the city combo box to the cities of the selected country.
            cityComboBox.ItemsSource = _locations.Where(c => c.Country == selectedCountry);
        }
        private void cityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string city = cityComboBox.SelectedValue.ToString();
            keyPointCity.SelectedValue = city;

        }

        private void keyPointCity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            keyPointCity.ItemsSource = _locations.Select(c => c.City).Distinct();
        }

        private void keyPointCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (keyPointCity.SelectedIndex != -1)
            {
                string city = keyPointCity.SelectedValue.ToString();
                List<KeyPoint> keyPointsShow = new List<KeyPoint>();


                foreach (Location l in _locations)
                {
                    if (city == l.City)
                    {
                        foreach (KeyPoint ky in _keyPoints)
                        {
                            if (ky.LocationId == l.Id)
                            {
                                keyPointsShow.Add(ky);
                            }
                        }
                    }
                }
                keyPointPlace.ItemsSource = keyPointsShow.Select(ky => ky.Place);
            }
        }

        private void AddAttractionClick(object sender, RoutedEventArgs e)
        {
            KeyPoint keyPoint = _keyPoints.Where(ky => ky.Place == keyPointPlace.SelectedValue.ToString()).FirstOrDefault();
            
            _tourKeyPoints.Add(keyPoint);

            // Clearing comboboxes after adding one keyPoint
            keyPointCity.SelectedIndex = -1;
            keyPointPlace.SelectedIndex = -1;

        }
    }
}
