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
using InitialProject.Controller;
using InitialProject.Model;
using InitialProject.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerView.xaml
    /// </summary>
    public partial class OwnerView : Window
    {
        private readonly User _owner;

        public OwnerView(User user)
        {
            InitializeComponent();
            DataContext = this;
            _owner = user;
        }

        private void ShowRegistrationAccommodationView_Click(object sender, RoutedEventArgs e)
        {
            AccommodationRegistrationView accommodationRegistrationView = new AccommodationRegistrationView(_owner);
            accommodationRegistrationView.Show();
        }

        private void ShowGuestRatingView_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
