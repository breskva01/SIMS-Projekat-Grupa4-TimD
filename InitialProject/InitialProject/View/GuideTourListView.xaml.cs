using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for GuideTourListView.xaml
    /// </summary>
    public partial class GuideTourListView : Window
    {
        private List<Tour> _tours;
        private List<Tour> _toursToday;
        private List<Location> _locations;

        private const string FilePathTour = "../../../Resources/Data/tours.csv";
        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Storage<Tour> _storageTour;
        private readonly Storage<Location> _storageLocation;

        private DateTime Today;

        private List<string> cityCountry { get; set; }

        public GuideTourListView(User user)
        {
            InitializeComponent();
            DataContext = this;

            Today = DateTime.Now;

            _toursToday = new List<Tour>();

            _storageTour = new Storage<Tour>(FilePathTour);
            _storageLocation = new Storage<Location>(FilePathLocation);

            _tours = _storageTour.Load();
            _locations = _storageLocation.Load();

            // Using LocationIds in list _tours creating objects Location
            foreach (Tour t in _tours)
            {
                foreach (Location l in _locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }

            // Checking which tours and happeing today and adding them to new list of tours
            foreach (Tour t in _tours)
            {
                if (t.Start.Year == Today.Year && t.Start.Month == Today.Month && t.Start.Day == Today.Day)
                {
                    _toursToday.Add(t);
                }
            }
            tourDataGrid.ItemsSource = _toursToday;
        }
        private void tourDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tour selectedTour = (Tour)tourDataGrid.SelectedItem;

            if (selectedTour != null)
            {
                //selectedTour.State = (TourState)Enum.Parse(typeof(TourState), "Started");
                selectedTour.State = 0;
            }
            TourLiveTrackingView tourLiveTrackingView = new TourLiveTrackingView(selectedTour);
            tourLiveTrackingView.Show();
        }
    }
}
