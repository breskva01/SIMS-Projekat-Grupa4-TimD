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
using InitialProject.View;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuideUserIntefaceView.xaml
    /// </summary>
    public partial class GuideUserIntefaceView : Window
    {
        User User { get; set; }
        public GuideUserIntefaceView(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
        }

        private void CreateTourClick(object sender, RoutedEventArgs e)
        {
            TourCreationView tourCreation = new TourCreationView(User);
            tourCreation.Show();
        }

        private void TourTrackingClick(object sender, RoutedEventArgs e)
        {
            GuideTourListView guideTourList = new GuideTourListView(User);
            guideTourList.Show();
        }

        private void AllToursClick(object sender, RoutedEventArgs e)
        {
            AllToursView allToursView = new AllToursView(User);
            allToursView.Show();
        }
    }
}
