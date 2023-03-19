using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for TourReservationWindow.xaml
    /// </summary>
    public partial class TourReservationWindow : Window, INotifyPropertyChanged
    {
        private Tour selectedTour;
        public ObservableCollection<Tour> Tours { get; set; }
        private User loggedInUser;
        private readonly TourReservationController reservationController;



        public TourReservationWindow(Tour tour, User user)
        {
            InitializeComponent();

            DataContext = this;
            loggedInUser = user;
            selectedTour = tour;
            
            tbTourName.Text = selectedTour.Name;
            tbTourDescription.Text = selectedTour.Description;
            tbTourAvailableSpots.Text = (selectedTour.MaximumGuests - selectedTour.CurrentNumberOfGuests).ToString();

            reservationController = new TourReservationController();

        }

        private int GetNumberOfGuests()
        {
            int NumberOfGuests = 0;
            try
            {
                NumberOfGuests = int.Parse(tbNumberOfGuests.Text);
            }
            catch { };

            return NumberOfGuests;
        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            int numberOfGuests = GetNumberOfGuests();
            if (numberOfGuests == 0)
            {
                MessageBox.Show("Please input a number of guests first.");
                return;
            }

            if (numberOfGuests + selectedTour.CurrentNumberOfGuests > selectedTour.MaximumGuests)
            { 
                MessageBox.Show("Unfortunately, there is not enough available spots for that many guests. Try lowering the guest number.");
                return;
            }

            TourReservation tourReservation = reservationController.CreateReservation(selectedTour.Id, loggedInUser.Id, numberOfGuests);

            Close();
            
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
