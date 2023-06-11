using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.GuestOne;
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

namespace InitialProject.WPF.NewViews
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*
            Height = SystemParameters.PrimaryScreenHeight * 0.82;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
            */
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainVM)
            {
                if (mainVM.CurrentViewModel is LayoutViewModel viewModel)
                    HandleLayoutPanelKeyDown(viewModel);
            }
        }

        private void HandleLayoutPanelKeyDown(LayoutViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D1))
                viewModel.NavigationBarViewModel.NavigateAccommodationBrowserCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D2))
                viewModel.NavigationBarViewModel.NavigateAnywhereAnytimeCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D3))
                viewModel.NavigationBarViewModel.NavigateMyResevationsCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D4))
                viewModel.NavigationBarViewModel.NavigateMyRequestsCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D5))
                viewModel.NavigationBarViewModel.NavigateRatingsCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D6))
                viewModel.NavigationBarViewModel.NavigateForumsCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Q))
                viewModel.NavigationBarViewModel.NavigateLoginCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.L))
                viewModel.StatusBarViewModel.ChangeLanguageCommand.Execute(null);
            else if (viewModel.ContentViewModel is ForumBrowserViewModel forumBrowserVM)
                HandleForumBrowserKeyDown(forumBrowserVM);
            else if (Keyboard.IsKeyDown(Key.Tab) && viewModel.ContentViewModel is AccommodationRatingViewModel ratingVM)
                ratingVM.SelectedTab++;
        }
        private void HandleForumBrowserKeyDown(ForumBrowserViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.Tab))
                viewModel.SelectedTabIndex++;
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
                viewModel.NavigateStartForumFormCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C) && Keyboard.IsKeyDown(Key.LeftShift))
                viewModel.CloseForumCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.O))
                viewModel.NavigateForumCommentsCommand.Execute(null);
        }
    }
}
