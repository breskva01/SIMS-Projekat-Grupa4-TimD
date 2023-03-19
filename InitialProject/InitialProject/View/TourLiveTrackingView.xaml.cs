using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for TourLiveTrackingView.xaml
    /// </summary>
    public partial class TourLiveTrackingView : Window
    {
        private Tour _tour;
        private const string FilePathKY = "../../../Resources/Data/keyPoints.csv";
        private const string FilePathLocation = "../../../Resources/Data/locations.csv";
        private List<KeyPoint> _keyPoints;
        private List<KeyPoint> _keyPointsFromSelectedTour;
        private List<Location> _locations;
        private readonly Storage<KeyPoint> _storageKeyPoint;
        private readonly Storage<Location> _storageLocation;
        private List<string> keyPointsShow;
        //private int numberOfActiveTours;
        private GuideTourListView tourListView;
        public TourLiveTrackingView(Tour tour, GuideTourListView guideTourListView)
        {
            InitializeComponent();
            DataContext = this;

            tourListView = guideTourListView;
            //numberOfActiveTours = NumberOfActiveTours;
            keyPointsShow = new List<string>();
            _tour = tour;
            _keyPointsFromSelectedTour = new List<KeyPoint>();
            _storageLocation = new Storage<Location>(FilePathLocation);
            _storageKeyPoint = new Storage<KeyPoint>(FilePathKY);
            _keyPoints = _storageKeyPoint.Load();
            _locations = _storageLocation.Load();

            foreach(int keyPointId in _tour.KeyPointIds)
            {
                foreach (KeyPoint ky in _keyPoints)
                {
                    if (ky.Id == keyPointId)
                    {
                        _keyPointsFromSelectedTour.Add(ky);
                    }
                }
            }
            foreach(Location l in _locations)
            {
                foreach(KeyPoint ky in _keyPointsFromSelectedTour)
                {
                    if(ky.LocationId == l.Id)
                    {
                        ky.Location = l;
                    }
                }
            }
            keyPointsDataGrid.ItemsSource = _keyPointsFromSelectedTour;

          
        }

        private void StopTourClick(object sender, RoutedEventArgs e)
        {
            _tour.State = (TourState)1;
            tourListView.NumberOfActiveTours = 0;
            tourListView.selectedTour.State = _tour.State;
            Close();
            
        }
    }
}
