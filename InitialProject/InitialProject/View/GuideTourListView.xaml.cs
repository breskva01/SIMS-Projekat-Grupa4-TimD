using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class GuideTourListView : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Tour> _tours;
        private ObservableCollection<Tour> _toursToday;
        private List<Location> _locations;

        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Storage<Location> _storageLocation;

        private DateTime _today;

        public int NumberOfActiveTours;

        public Tour SelectedTour;

        private readonly TourController _controller;

        TourLiveTrackingView tourLiveTrackingView;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GuideTourListView(User user)
        {
            InitializeComponent();
            DataContext = this;

           // SelectedTour = new Tour();
            _today = DateTime.Now;

            _toursToday = new ObservableCollection<Tour>();
            NumberOfActiveTours = 0;

            _storageLocation = new Storage<Location>(FilePathLocation);
            _locations = _storageLocation.Load();

            _controller = new TourController();
            _tours = new ObservableCollection<Tour>(_controller.GetAll());

            // Using LocationIds in list _tours creating objects Location
            foreach (Tour t in _tours)
            {
                foreach (Location l in _locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }

            // Adding tours which are happeing today to new list of tours
            foreach (Tour t in _tours)
            {
                if (IsDateToday(t))
                {
                    _toursToday.Add(t);
                }
            }
            tourDataGrid.ItemsSource = _toursToday;

        }
        // Checking which tours and happeing today
        private bool IsDateToday(Tour t)
        {
            return t.Start.Year == _today.Year && t.Start.Month == _today.Month && t.Start.Day == _today.Day;
        }
        /*
        private void tourDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Checking if there is any active tour
            if (NumberOfActiveTours != 0)
            {
                return;
            }
            SelectedTour = (Tour)tourDataGrid.SelectedItem;

            if (SelectedTour.State == (TourState)TourState.None || SelectedTour.State == (TourState)TourState.Started)
            {
                if (SelectedTour != null)
                {
                    SelectedTour.State = (TourState)TourState.Started;
                    _controller.Update(SelectedTour);
                }
                NumberOfActiveTours++;
                tourDataGrid.SelectedIndex = -1;
                TourLiveTrackingView tourLiveTrackingView = new TourLiveTrackingView(SelectedTour, this);
                tourLiveTrackingView.Show();
            }

        }*/

        private void StartTourClick(object sender, RoutedEventArgs e)
        {
            if (NumberOfActiveTours != 0)
            {
                return;
            }
            SelectedTour = (Tour)tourDataGrid.SelectedItem;

            if (SelectedTour.State == (TourState)TourState.None || SelectedTour.State == (TourState)TourState.Started)
            {
                if (SelectedTour != null)
                {
                    SelectedTour.State = (TourState)TourState.Started;
                    _controller.Update(SelectedTour);
                }
                NumberOfActiveTours++;
                tourDataGrid.SelectedIndex = -1;
                TourLiveTrackingView tourLiveTrackingView = new TourLiveTrackingView(SelectedTour, this);
                tourLiveTrackingView.Show();
            }
        }

        private void tourDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
