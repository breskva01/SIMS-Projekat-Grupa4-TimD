using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.GuestOne;
using OxyPlot;
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
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Left))
                {
                    if (mainVM.CurrentViewModel is AccommodationReservationViewModel vM1)
                        vM1.NavigateAccommodationBrowserCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is ImageBrowserViewModel vM2)
                        vM2.NavigateReservationFormCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is AccommodationReservationDatePickerViewModel vM3)
                        vM3.OpenReservationFormCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is AccommodationReservationDetailsViewModel vM4)
                        vM4.NavigateAnywhereAnytimeCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is AccommodationReservationMoveRequestViewModel vM5)
                        vM5.NavigateMyResevationsCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is ViewModels.GuestOne.ForumCommentsViewModel vM6)
                        vM6.NavigateBackCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is GuestRatingDetailsViewModel vM7)
                        vM7.NavigateRatingsCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is OwnerCommentViewModel vM8)
                        vM8.NavigateMyRequestsCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is RateAccommodationViewModel vM9)
                        vM9.NavigateRatingsCommand.Execute(null);
                    else if (mainVM.CurrentViewModel is StartForumViewModel vM10)
                        vM10.NavigateBackCommand.Execute(null);
                }
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
