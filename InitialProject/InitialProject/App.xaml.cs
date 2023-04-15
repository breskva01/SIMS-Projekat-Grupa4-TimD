using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.WPF.NewViews;
using InitialProject.Application.Stores;
using InitialProject.Application.Services;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly NavigationStore _navigationStore;
        public App()
        {
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateTourBrowserViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private TourReservationViewModel CreateTourReservationViewModel()
        {
            return new TourReservationViewModel(new NavigationService(_navigationStore, CreateTourBrowserViewModel));
        }

        private TourBrowserViewModel CreateTourBrowserViewModel()
        {
            return new TourBrowserViewModel(new NavigationService(_navigationStore, CreateTourReservationViewModel));
        }

    }
}
