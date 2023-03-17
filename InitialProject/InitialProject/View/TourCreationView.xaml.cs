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
        //public ObservableCollection<string> MyList { get; set; }
        private List<City> cities;
        private List<KeyPoint> keyPoints;
        private readonly Storage<City> _storageCity;
        private readonly Storage<KeyPoint> _storageKeyPoint;
        private const string FilePath = "../../../Resources/Data/cities.csv";
        private const string FilePathKY = "../../../Resources/Data/keyPoints.csv";
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
        private List<KeyPoint> tourKeyPoints = new List<KeyPoint>();
        private List<KeyPoint> attractions = new List<KeyPoint>();
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCreationView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _storageCity = new Storage<City>(FilePath);
            _storageKeyPoint = new Storage<KeyPoint>(FilePathKY);
            keyPoints = _storageKeyPoint.Load();
            cities = _storageCity.Load();

            // Set the items source of the country combo box to the distinct list of countries.
            countryComboBox.ItemsSource = cities.Select(c => c.Country).Distinct();
            keyPointCity.ItemsSource = cities.Select(c => c.Name).Distinct();
            //countryComboBox1.ItemsSource = keyPoints.Select(k => k.Attraction).Distinct();

            _tourController = new TourController();
            //MyList = new ObservableCollection<string> { "Zela", "Spale", "Papi" };
            //DataContext = MyList;
            //combo.ItemsSource = MyList;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void TourCreationClick(object sender, RoutedEventArgs e)
        {
            if(tourKeyPoints.Count() > 1)
            {
                City City = new City();
                City.Country = Country;
                City.Name = Town;
                City.CityId = cities.Where(c => c.Name == Town).Select(c => c.CityId).FirstOrDefault();
                int TourDuration = int.Parse(Duration);
                int MaxGuests = int.Parse(MaximumGuests);
                GuideLanguage lang = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), LanguageType);
                _tourController.CreateTour(TourName, City, Description, lang, MaxGuests, Start, TourDuration, PictureUrl, tourKeyPoints);
                MessageBox.Show("Tura uspesno kreirana");
            }
            else
            {
                MessageBox.Show("Mora biti najmanje dve kljucne tacke");
            }

        }
        /*
        public int NextId()
        {
            cities = _storage.Load();
            if (cities.Count < 1)
            {
                return 1;
            }
            return cities.Max(t => t.CityId) + 1;
        }*/
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
            cityComboBox.ItemsSource = cities.Where(c => c.Country == selectedCountry);
        }

        private void KeyPointCity(object sender, TextCompositionEventArgs e)
        {
            keyPointCity.ItemsSource = cities.Select(c => c.Name).Distinct();
        }

        private void KeyPointAttraction(object sender, TextCompositionEventArgs e)
        {
           
        }

        private void cityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string city = cityComboBox.SelectedValue.ToString();
            keyPointCity.SelectedValue = city;
            
            //keyPointCity.Text = cityComboBox.SelectedValue.ToString();
            /*Console.WriteLine(keyPointCity.Inde);
            Console.WriteLine(keyPointCity.SelectedValue);*/
        }

        private void keyPointCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (keyPointCity.SelectedIndex != -1)
            {
                string city = keyPointCity.SelectedValue.ToString();
                List<KeyPoint> attractions = new List<KeyPoint>();


                foreach (City c in cities)
                {
                    if (city == c.Name)
                    {
                        foreach (KeyPoint ky in keyPoints)
                        {
                            if (ky.CityId == c.CityId)
                            {
                                attractions.Add(ky);
                            }
                        }
                    }
                }
                keyPointAttraction.ItemsSource = attractions.Select(a => a.Attraction);
            }
        }

        private void Add_Attraction_Click(object sender, RoutedEventArgs e)
        {
            KeyPoint ky = keyPoints.Where(ky => ky.Attraction == keyPointAttraction.SelectedValue.ToString()).FirstOrDefault();
            
            tourKeyPoints.Add(ky);
            Console.WriteLine(ky.Id);
            keyPointCity.SelectedIndex = -1;
            keyPointAttraction.SelectedIndex = -1;

        }

        private void keyPointAttraction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
