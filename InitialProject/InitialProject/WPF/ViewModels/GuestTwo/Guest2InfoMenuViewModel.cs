using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class Guest2InfoMenuViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;

        public ICommand TourRatingCommand { get; }
        public ICommand MyVouchersCommand { get; }
        public ICommand MyRequestsCommand { get; }
        public ICommand MyComplexRequestsCommand { get; }
        public ICommand MyNotificationsCommand { get; }
        public ICommand MenuCommand { get; }
        public Guest2InfoMenuViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            TourRatingCommand = new TourRatingCommand(ShowTourRatingView, user);
            MyVouchersCommand = new ExecuteMethodCommand(ShowMyVouchersView);
            MyRequestsCommand = new ExecuteMethodCommand(ShowMyRequestsView);
            MyComplexRequestsCommand = new ExecuteMethodCommand(ShowMyComplexRequestsView);
            MyNotificationsCommand = new ExecuteMethodCommand(ShowNotificationsView);
            MenuCommand = new ExecuteMethodCommand(ShowMainMenuView);
        }

        private void ShowTourRatingView()
        {
            TourRatingViewModel tourRatingViewModel = new TourRatingViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRatingViewModel));

            navigate.Execute(null);
        }

        private void ShowMyRequestsView()
        {
            TourRequestBrowserViewModel tourRequestBrowserViewModel = new TourRequestBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestBrowserViewModel));

            navigate.Execute(null);
        }

        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }

        
        private void ShowMyComplexRequestsView()
        {
            ComplexTourRequestBrowserViewModel complexTourRequestBrowserViewModel = new ComplexTourRequestBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, complexTourRequestBrowserViewModel));

            navigate.Execute(null);
        }
        /*
        private void ShowRequestStatsView()
        {
            TourRequestStatsViewModel tourRequestStatsViewModel = new TourRequestStatsViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStatsViewModel));

            navigate.Execute(null);
        }
        */
        private void ShowMyVouchersView()
        {
            MyVouchersViewModel myVouchersViewModel = new MyVouchersViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, myVouchersViewModel));

            navigate.Execute(null);
        }


        private void ShowMainMenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));

            navigate.Execute(null);
        }
    }
}
