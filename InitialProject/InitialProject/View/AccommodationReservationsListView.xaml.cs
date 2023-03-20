using InitialProject.Controller;
using System;
using System.Collections.Generic;
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
using InitialProject.Model;
using InitialProject.FileHandler;
using System.Collections.ObjectModel;
using InitialProject.Observer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationsListView.xaml
    /// </summary>
    public partial class AccommodationReservationsListView : Window, IObserver, INotifyPropertyChanged
    {
        private readonly AccommodationReservationController _reservationController;
        private readonly AccommodationController _accommodationController;
        private readonly UserController _userController;
        private readonly User _owner;
        public readonly Accommodation Accommodation;
        private List<Accommodation> _accommodations;
        private List<User> _users;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get;  set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public AccommodationReservationsListView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _owner = user;
            _reservationController = new AccommodationReservationController();
            _reservationController.Subscribe(this);
            _accommodationController = new AccommodationController();
            _userController = new UserController();
            _accommodations = _accommodationController.GetAll();
            _users = _userController.GetUsers();
            AccommodationReservations = new ObservableCollection<AccommodationReservation>(_reservationController.FindCompletedAndUnrated(_owner.Id));
        }

        private void RateGuests_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation == null)
            {
                MessageBox.Show("You haven't selected a guest!");
            }
            else
            {
                SelectedReservation.Accommodation.OwnerId = _owner.Id;
                GuestRatingView guestRatingView = new GuestRatingView(SelectedReservation);
                guestRatingView.Show();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Update()
        {
            UpdateReservations();
        }
        private void UpdateReservations()
        {
            AccommodationReservations.Clear();
            foreach (var reservation in _reservationController.FindCompletedAndUnrated(_owner.Id))
            {
                AccommodationReservations.Add(reservation);
            }
        }
    }
}
