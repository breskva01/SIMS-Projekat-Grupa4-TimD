using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private const string FilePathKeyPoint = "../../../Resources/Data/keyPoints.csv";
        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly TourController _controller;


        private List<KeyPoint> _keyPoints;
        private List<KeyPoint> _keyPointsFromSelectedTour;
        private List<Location> _locations;
        private List<Tour> _tours;

        private readonly Storage<KeyPoint> _storageKeyPoint;
        private readonly Storage<Tour> _storageTour;
        private readonly Storage<Location> _storageLocation;

        private GuideTourListView tourListView;

        private int numberOfKeyPointsFromSelectedTour;

        public TourLiveTrackingView(Tour tour, GuideTourListView guideTourListView)
        {
            InitializeComponent();
            DataContext = this;

            _tour = tour;

            _controller = new TourController();
            _tours = new List<Tour>(_controller.GetAll());

            _storageLocation = new Storage<Location>(FilePathLocation);
            _storageKeyPoint = new Storage<KeyPoint>(FilePathKeyPoint);
            
            _keyPoints = _storageKeyPoint.Load();
            _locations = _storageLocation.Load();

            _keyPointsFromSelectedTour = new List<KeyPoint>();

            tourListView = guideTourListView;
            
            // Adding KeyPoints to new list that contains only keyPoints from selected tour
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

            // Initializing Location objects in KeyPoint objects based on LocationId
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

            numberOfKeyPointsFromSelectedTour = _keyPointsFromSelectedTour.Count();
            keyPointsDataGrid.ItemsSource = _keyPointsFromSelectedTour;
        }

        private void StopTourClick(object sender, RoutedEventArgs e)
        {
            _tour.State = (TourState)2;
            tourListView.NumberOfActiveTours = 0;
            tourListView.selectedTour.State = _tour.State;
            _controller.Update(tourListView.selectedTour);
            Close();
            
        }

        private void keyPointsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyPoint selectedKeyPoint = (KeyPoint)keyPointsDataGrid.SelectedItem;
            selectedKeyPoint.Visited = true;
            numberOfKeyPointsFromSelectedTour--;
            if(numberOfKeyPointsFromSelectedTour == 0)
            {
                _tour.State = (TourState)3;
                tourListView.NumberOfActiveTours = 0;
                tourListView.selectedTour.State = _tour.State;
                _controller.Update(tourListView.selectedTour);

                Close();
            }
        }
    }
}
