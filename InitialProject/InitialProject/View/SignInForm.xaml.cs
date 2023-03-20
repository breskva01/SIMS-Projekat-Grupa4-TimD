using InitialProject.Controller;
using InitialProject.FileHandler;
using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Model.DAO;
using InitialProject.Repository;
using InitialProject.Storage;
using InitialProject.View;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserController _userController;
        private readonly AccommodationReservationController _reservationController;
        private readonly UserRepository _repository;
        private readonly Storage<Accommodation> _accommodationStorage;
        private List<AccommodationReservation> _reservations;
        private List<Accommodation> _accommodations;
        private const string accommodationsFilePath = "../../../Resources/Data/accommodations.csv";

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
            _userController = new UserController();
            _reservationController = new AccommodationReservationController();
            _accommodationStorage = new Storage<Accommodation>(accommodationsFilePath);
            _reservations = new List<AccommodationReservation>();
            _accommodations = _accommodationStorage.Load();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userController.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    OpenAppropriateWindow(user);
                    Close();
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
        private void OpenAppropriateWindow(User user)
        {
            switch (user.UserType)
            {
                // TO DO: otvoriti odgovarajuce prozore za svaki tip korisnika
                case UserType.Owner:
                    {
                        int unratedGuests = 0;
                        _reservations = _reservationController.FindCompletedAndUnratedReservations(user.Id);
                        foreach (AccommodationReservation res in _reservations)
                        {
                            if (DateOnly.FromDateTime(DateTime.Now) > res.LastNotification)
                            {
                                _reservationController.updateLastNotification(res);
                                unratedGuests++;
                            }
                                
                        }
                        if (unratedGuests > 0)
                            MessageBox.Show("You have " + unratedGuests.ToString() + " unrated guests!");

                        OwnerView ownerView = new OwnerView(user);
                        ownerView.Show();
                        break;
                    }
                case UserType.Guest1:
                    {
                        AccommodationBrowser accommodationBrowser = new AccommodationBrowser(user);
                        accommodationBrowser.Show();
                        break;
                    }
                case UserType.TourGuide:
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        break;
                    }
                case UserType.Guest2:
                    {
                        CommentsOverview commentsOverview = new CommentsOverview(user);
                        commentsOverview.Show();
                        break;
                    }
            }
        }
    }
}
