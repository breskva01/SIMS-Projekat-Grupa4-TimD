using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.Application.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TourReservationWindow.xaml
    /// </summary>
    public partial class TourReservationWindow : Window, INotifyPropertyChanged
    {
        private Tour _selectedTour;
        public ObservableCollection<Tour> Tours { get; set; }
        private readonly TourReservationController _reservationController;

        private User _loggedInUser;

        private Regex _notNumber = new Regex(@"\D+");


        public TourReservationWindow(Tour tour, User user)
        {
            InitializeComponent();
            DataContext = this;

            _loggedInUser = user;
            _selectedTour = tour;
            
            tbTourName.Text = _selectedTour.Name;
            tbTourDescription.Text = _selectedTour.Description;
            tbTourAvailableSpots.Text = (_selectedTour.MaximumGuests - _selectedTour.CurrentNumberOfGuests).ToString();

            _reservationController = new TourReservationController();
        }

        private int GetNumberOfGuests()
        {
            Match match = _notNumber.Match(tbNumberOfGuests.Text);
            if (match.Success)
            {
                MessageBox.Show("You cannot enter non-number character in the Number of guests box.");
            }

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

            if (numberOfGuests + _selectedTour.CurrentNumberOfGuests > _selectedTour.MaximumGuests)
            { 
                MessageBox.Show("Unfortunately, there is not enough available spots for that many guests. Try lowering the guest number.");
                return;
            }

            TourReservation tourReservation = _reservationController.CreateReservation(_selectedTour.Id, _loggedInUser.Id, numberOfGuests);

            Close();
            
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
