using InitialProject.Controller;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AccommodationReservationDatePicker.xaml
    /// </summary>
    public partial class AccommodationReservationDatePicker : Window
    {
        private readonly AccommodationReservationController _controller;
        public AccommodationReservation SelectedReservation { get; set; }
        public List<AccommodationReservation> Reservations { get; set; }
        public int GuestNumber { get; set; }
        public AccommodationReservationDatePicker(AccommodationReservationController controller, List<AccommodationReservation> reservations)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;

            Height = SystemParameters.PrimaryScreenHeight * 0.55;
            Width = SystemParameters.PrimaryScreenWidth * 0.55;
            Reservations = reservations;
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation == null)
                MessageBox.Show("Izaberite željeni termin.");
            else if (GuestNumber == 0)
                MessageBox.Show("Unesite broj gostiju.");
            else if (GuestNumber > SelectedReservation.Accommodation.MaximumGuests)
                MessageBox.Show("Uneti broj gostiju prelazi zadati limit.");
            else
            {
                SelectedReservation.GuestNumber = GuestNumber;
                _controller.Save(SelectedReservation);
                MessageBox.Show("Rezervacija uspešno kreirana.");
                Close();
            }
        }
    }
}
