using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Storage<Location> _storageLocation;

        private DateTime _today;

        public int NumberOfActiveTours;

        public Tour SelectedTour;

        private readonly TourController _controller;

        private List<string> _cityCountry { get; set; }
        TourLiveTrackingView tourLiveTrackingView;

        public GuideTourListView(User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedTour = new Tour();
            _today = DateTime.Now;

            _toursToday = new List<Tour>();
            NumberOfActiveTours = 0;

            _storageLocation = new Storage<Location>(FilePathLocation);
            _locations = _storageLocation.Load();

            _controller = new TourController();
            _tours = new List<Tour>(_controller.GetAll());

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
                if (t.Start.Year == _today.Year && t.Start.Month == _today.Month && t.Start.Day == _today.Day)
                {
                    _toursToday.Add(t);
                }
            }
            tourDataGrid.ItemsSource = _toursToday;

        }
        private void tourDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Checking if there is any active tour
            if (NumberOfActiveTours == 0)
            {
                SelectedTour = (Tour)tourDataGrid.SelectedItem;

                if (SelectedTour.State == 0 || SelectedTour.State == (TourState)1)
                {
                    if (SelectedTour != null)
                    {
                        SelectedTour.State = (TourState)1;
                        _controller.Update(SelectedTour);
                    }
                    NumberOfActiveTours++;
                    tourDataGrid.SelectedIndex = -1;
                    TourLiveTrackingView tourLiveTrackingView = new TourLiveTrackingView(SelectedTour, this);
                    tourLiveTrackingView.Show();
                }
            }

        }
    }
}
