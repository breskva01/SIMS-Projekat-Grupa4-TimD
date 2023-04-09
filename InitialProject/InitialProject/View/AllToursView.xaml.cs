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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AllToursView.xaml
    /// </summary>
    public partial class AllToursView : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Tour> ToursShow { get; set; }

        private readonly TourController _controller;

        private const string FilePathLocation = "../../../Resources/Data/locations.csv";

        private readonly Storage<Location> _storageLocation;

        private List<Location> _locations;
        public Tour SelectedTour { get; set; }

        private DateTime _today;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AllToursView(User user)
        {
            InitializeComponent();
            DataContext = this;

            _controller = new TourController();
            ToursShow = new ObservableCollection<Tour>();

            Tours = new ObservableCollection<Tour>(_controller.GetAll());

            _storageLocation = new Storage<Location>(FilePathLocation);
            _locations = _storageLocation.Load();

            _today = DateTime.Now;

            foreach (Tour t in Tours)
            {
                foreach (Location l in _locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            foreach(Tour t in Tours)
            {
                if(t.State == 0)
                {
                    ToursShow.Add(t);
                }
            }

        }

        private void CancelTourClick(object sender, RoutedEventArgs e)
        {
            SelectedTour = (Tour)lbTours.SelectedItem;
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");
                return;
            }
            if(SelectedTour.Start > _today.AddHours(48))
            {
                if (SelectedTour.State == TourState.None)
                {
                    SelectedTour.State = TourState.Canceled;
                    ToursShow.Remove(SelectedTour);
                    _controller.Update(SelectedTour);
                    return;
                }
            }
            MessageBox.Show("It's too late to cancel this tour.");
            return;

        }
    }
}
