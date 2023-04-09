using InitialProject.Controller;
using InitialProject.Domain.Models;
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

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationReservation.xaml
    /// </summary>
    public partial class AccommodationReservationWindow : Window
    {
        private readonly AccommodationReservationController _controller;
        public Accommodation Accommodation { get; set; }
        public User Guest { get; set; }
        public int Days { get; set; }
        public AccommodationReservationWindow(User user, Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            Guest = user;
            Accommodation = accommodation;
            _controller = new AccommodationReservationController();

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
                DateTime startDate = (DateTime) startDatePicker.SelectedDate;
                DateTime endDate = (DateTime) endDatePicker.SelectedDate;
                List<AccommodationReservation> reservations = _controller.FindAvailable(startDate, endDate, Days, Accommodation, Guest);
                AccommodationReservationDatePicker datePicker = new AccommodationReservationDatePicker(_controller, reservations);
                datePicker.ShowDialog();
            }
            else
                MessageBox.Show("Izaberite željeni opseg datuma");
        }
    }
}
