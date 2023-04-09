using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Observer;
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
    /// Interaction logic for MyAccommodationReservations.xaml
    /// </summary>
    public partial class MyAccommodationReservations : Window, INotifyPropertyChanged, IObserver
    {
        public User LoggedInUser { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        private readonly AccommodationReservationController _controller;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MyAccommodationReservations(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            _controller = new AccommodationReservationController();
            Reservations = new ObservableCollection<AccommodationReservation>
                                (_controller.GetConfirmed(LoggedInUser.Id));
            _controller.Subscribe(this);

            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }
        public void Update()
        {
            var reservations = new ObservableCollection<AccommodationReservation>
                                (_controller.GetConfirmed(LoggedInUser.Id));
            Reservations.Clear();
            foreach (var r in reservations)
                Reservations.Add(r);
        }

        private void CancelReservationClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            AccommodationReservation reservation = (AccommodationReservation)clickedButton.DataContext;
            if (_controller.Cancel(reservation.Id))
                MessageBox.Show("Rezervacija uspešno otkazana.");
            else
                MessageBox.Show("Rezervacija se ne može otkazati.");
        }
    }
}
