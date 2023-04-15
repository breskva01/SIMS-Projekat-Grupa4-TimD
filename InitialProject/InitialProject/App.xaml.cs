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
        private readonly ViewModelService _viewModelService;
        public App()
        {
            _navigationStore = new NavigationStore();
            _viewModelService = new ViewModelService(_navigationStore);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = _viewModelService.CreateSignInViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }


    }
}
