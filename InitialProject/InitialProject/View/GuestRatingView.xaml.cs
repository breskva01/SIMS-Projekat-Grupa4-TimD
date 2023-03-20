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
using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuestRatingView.xaml
    /// </summary>
    public partial class GuestRatingView : Window, INotifyPropertyChanged
    {
        private readonly GuestRatingController _guestRatingController;
        private readonly UserController _userController;
        private AccommodationReservation _selectedReservation;
        private readonly AccommodationReservationController _reservationController;
        private const string FilePath = "../../../Resources/Data/guestRatings.csv";

        private string _hygiene;
        public string Hygiene
        {
            get => _hygiene;
            set
            {
                if (value != _hygiene)
                {
                    _hygiene = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _respectsRules;
        public string RespectsRules
        {
            get => _respectsRules;
            set
            {
                if (value != _respectsRules)
                {
                    _respectsRules = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set 
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuestRatingView(AccommodationReservation SelectedReservation)
        {
            InitializeComponent();
            DataContext = this;
            _selectedReservation = SelectedReservation;
            _userController= new UserController();
            _guestRatingController= new GuestRatingController();
            _reservationController = new AccommodationReservationController();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RateGuest_Click(object sender, RoutedEventArgs e)
        {
            int ownerId = _selectedReservation.Accommodation.OwnerId;
            int guestId = _selectedReservation.GuestId;
            GuestRating guestRating = _guestRatingController.RateGuest(ownerId, guestId, int.Parse(Hygiene), int.Parse(RespectsRules), Comment);
            //_reservationController.updateRatingStatus(_selectedReservation);
            _userController.AddGuestRating(guestId, guestRating);
            Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
