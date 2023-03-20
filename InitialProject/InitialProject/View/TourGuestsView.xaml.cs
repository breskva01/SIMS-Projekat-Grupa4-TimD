using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
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
    /// Interaction logic for TourGuestsView.xaml
    /// </summary>
    public partial class TourGuestsView : Window
    {
        private const string FilePathUser = "../../../Resources/Data/users.csv";

        private List<User> _guests;
        private List<User> _users;

        private readonly TourReservationController _reservationController;

        private List<TourReservation> _reservations;

        private readonly Storage<User> _storageUser;
        public TourGuestsView()
        {
            InitializeComponent();
            DataContext = this;

            _guests = new List<User>();

            _reservationController = new TourReservationController();
            _reservations = new List<TourReservation>(_reservationController.GetAll());

            _storageUser = new Storage<User>(FilePathUser);
            _users = _storageUser.Load();

            foreach (TourReservation res in _reservations)
            {
                foreach (User u in _users)
                {
                    if (res.GuestId == u.Id)
                    {
                        _guests.Add(u);
                    }

                }
            }
            guestsDataGrid.ItemsSource = _guests;
        }
    }
}