using InitialProject.Controller;
using InitialProject.Model;
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
        private readonly TourController _tourController;
        private readonly LocationController _locationController;
        public ObservableCollection<Location> Locations { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if(value != _name)
                {
                    _name = value;
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
        private int _maximumGuests;
        public int MaximumGuests
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCreationView(User user, TourController TourController, LocationController LocationController)
        {
            InitializeComponent();
            DataContext = this;
            
            _tourController = TourController;
            _locationController = LocationController;
            Locations = new ObservableCollection<Location>(_locationController.GetAll());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void TourCreationClick(object sender, RoutedEventArgs e)
        {
           //Location Location = _locationController.Create(Country, City);

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

    }
}
