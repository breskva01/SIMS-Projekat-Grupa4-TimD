using InitialProject.WPF.ViewModels.GuestOne;
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
            Height = SystemParameters.PrimaryScreenHeight * 0.75;
            Width = SystemParameters.PrimaryScreenWidth * 0.75;
            */
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainVM)
            {
                if (mainVM.CurrentViewModel is LayoutViewModel viewModel)
                    HandleLayoutPanelKeyDown(viewModel);
                else if (mainVM.CurrentViewModel is OwnerMainMenuViewModel ownerMainMenuViewModel)
                    HandleOwnerMainMenuPanelKeydown(ownerMainMenuViewModel);
                else if (mainVM.CurrentViewModel is OwnerProfileViewModel ownerProfileViewModel)
                    HandleOwnerProfilePanelKeydown(ownerProfileViewModel);
                else if (mainVM.CurrentViewModel is AccommodationsViewModel accommodationsViewModel)
                    HandleAccommodationsPanelKeydown(accommodationsViewModel);
                else if (mainVM.CurrentViewModel is ReservationMoveRequestsViewModel reservationMoveRequestsViewModel)
                    HandleMoveRequestPanelKeydown(reservationMoveRequestsViewModel);
                else if (mainVM.CurrentViewModel is ForumSearchViewModel forumSearchViewModel)
                    HandleForumsPanelKeydown(forumSearchViewModel);
                else if (mainVM.CurrentViewModel is ForumCommentsViewModel forumCommentsViewModel)
                    HandleForumCommentsPanelKeydown(forumCommentsViewModel);
                else if(mainVM.CurrentViewModel is GuestRatingViewModel guestRatingViewModel)
                    HandleGuestRatingPanelKeyDown(guestRatingViewModel);
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
            /*else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D6))
                viewModel.NavigationBarViewModel.NavigateForumsCommand.Execute(null);*/
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Q))
                viewModel.NavigationBarViewModel.NavigateLoginCommand.Execute(null);
           /* else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.L))
                viewModel.StatusBarViewModel.ChangeLanguageCommand.Execute(null);
            else if (viewModel.ContentViewModel is ForumBrowserViewModel forumBrowserVM)
                HandleForumBrowserKeyDown(forumBrowserVM);*/
            else if (Keyboard.IsKeyDown(Key.Tab) && viewModel.ContentViewModel is AccommodationRatingViewModel ratingVM)
                ratingVM.SelectedTab++;
        }
        private void HandleOwnerMainMenuPanelKeydown(OwnerMainMenuViewModel viewModel)
        {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.H))
                viewModel.ViewHelpCommand.Execute(null);
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.P))
                viewModel.ViewProfileCommand.Execute(null);
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.L))
                viewModel.SignOutCommand.Execute(null);
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F))
                viewModel.ViewForumsCommand.Execute(null);
        }
        private void HandleOwnerProfilePanelKeydown(OwnerProfileViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
                viewModel.ShowNotificationsViewCommand.Execute(null);
            else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                viewModel.ShowMyAccommodationsViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                viewModel.ShowMyRatingsViewCommand.Execute(null);
            else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.G))
                viewModel.ShowRateGuestsViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.L))
                viewModel.SignOutCommand.Execute(null);
        }
        private void HandleAccommodationsPanelKeydown(AccommodationsViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N) && Keyboard.IsKeyDown(Key.A))
                viewModel.ShowAccommodationRegistrationViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                viewModel.ShowAccommodationStatisticsViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                viewModel.ShowReservationMoveRequestsViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.R))
                viewModel.ShowScheduleRenovationViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A) && Keyboard.IsKeyDown(Key.R))
                viewModel.ShowMyRenovationAppointmentsViewCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
        }
        private void HandleMoveRequestPanelKeydown(ReservationMoveRequestsViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
                viewModel.DenyCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                viewModel.ApproveCommand.Execute(null);
        }
        private void HandleForumsPanelKeydown(ForumSearchViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.V))
                viewModel.ViewForumCommand.Execute(null);
        }
        private void HandleForumCommentsPanelKeydown(ForumCommentsViewModel viewModel)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                viewModel.ReportCommentCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                viewModel.SubmitCommentCommand.Execute(null);
        }
        private void HandleGuestRatingPanelKeyDown(GuestRatingViewModel viewModel)  
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.B))
                viewModel.BackCommand.Execute(null);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
                viewModel.RateGuestCommand.Execute(null);
        }
    }
}
