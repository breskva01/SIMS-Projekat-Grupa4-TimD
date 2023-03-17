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
    public partial class AccommodationReservation : Window, INotifyPropertyChanged
    {
        private Accommodation _accommodation;
        public Accommodation Accommodation
        {
            get => _accommodation;
            set
            {
                _accommodation = value;
                OnPropertyChanged();
            }
        }
        public User LoggedInUser { get; set; }
        private readonly AccommodationController _controller;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationReservation(AccommodationController controller, User user, Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            _controller = controller;
            LoggedInUser = user;
            Accommodation = accommodation;

            Height = SystemParameters.PrimaryScreenHeight * 0.5;
            Width = SystemParameters.PrimaryScreenWidth * 0.65;
        }
    }
}
