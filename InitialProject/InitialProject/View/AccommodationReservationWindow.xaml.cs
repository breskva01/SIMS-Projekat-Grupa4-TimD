using InitialProject.Controller;
using InitialProject.Model;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservation.xaml
    /// </summary>
    public partial class AccommodationReservationWindow : Window
    {
        private readonly AccommodationReservationController _reservationController;
        public Accommodation Accommodation { get; set; }
        public User LoggedInUser { get; set; }
        public int Days { get; set; }
        public AccommodationReservationWindow(User user, Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            Accommodation = accommodation;
            _reservationController = new AccommodationReservationController();

            Height = SystemParameters.PrimaryScreenHeight * 0.5;
            Width = SystemParameters.PrimaryScreenWidth * 0.65;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if (Days == 0)
                MessageBox.Show("Unesite željeni broj dana.");
            else if (Days < Accommodation.MinimumDays)
                MessageBox.Show($"Minimalani broj dana: {Accommodation.MinimumDays}");
            else if (startDatePicker.SelectedDate != null && endDatePicker.SelectedDate != null)
            {
                DateTime startDate = (DateTime)startDatePicker.SelectedDate;
                DateTime endDate = (DateTime)endDatePicker.SelectedDate;
                List<AccommodationReservation> reservations = _reservationController.FindAvailable(startDate, endDate, Days, Accommodation, LoggedInUser);
                AccommodationReservationDatePicker datePicker = new AccommodationReservationDatePicker(_reservationController, reservations);
                datePicker.ShowDialog();
            }
            else
                MessageBox.Show("Izaberite željeni opseg datuma");
        }
    }
}
