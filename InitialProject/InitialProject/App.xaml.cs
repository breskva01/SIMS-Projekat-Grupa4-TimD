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
using InitialProject.Application.Injector;

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
            RepositoryInjector.MapRepositories();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new SignInViewModel(_navigationStore);

            //Adding global styles to app resources
            var stylesResourceDictionary = new ResourceDictionary();
            stylesResourceDictionary.Source = new Uri("Resources/Styles/Styles.xaml", UriKind.RelativeOrAbsolute);
            Resources.MergedDictionaries.Add(stylesResourceDictionary);

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
