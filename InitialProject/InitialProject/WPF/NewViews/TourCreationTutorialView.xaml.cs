using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for TourCreationTutorialView.xaml
    /// </summary>
    public partial class TourCreationTutorialView : Window
    {
        public TourCreationTutorialView(NavigationStore navigationStore, User user)
        {
            InitializeComponent();
            TourCreationTutorialViewModel viewModel = new TourCreationTutorialViewModel(navigationStore, user, this);
            tutorialPlayer.Source = new Uri("C:/Users/lukaz/OneDrive/Desktop/Videos/pls.mkv");
        }
    }
}
