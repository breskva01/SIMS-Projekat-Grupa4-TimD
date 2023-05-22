using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels.GuestTwo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class Guest2MenuViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;

        public ICommand TourBrowserCommand { get; }
        public ICommand TrackingCommand { get; }
        public ICommand MyReservationsCommand { get; }
        public ICommand RequestMenuCommand { get; }
        public ICommand MyInfoCommand { get; }
        

        public ICommand SignOutCommand { get; }
        public Guest2MenuViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            TourBrowserCommand = new ExecuteMethodCommand(ShowTourBrowserView);
            TrackingCommand = new TourTrackingCommand(ShowTourTrackingView, user);
            //MyReservationsCommand = new ExecuteMethodCommand(ShowMyReservationsView, user);
            RequestMenuCommand = new ExecuteMethodCommand(ShowRequestMenuView);
            MyInfoCommand = new ExecuteMethodCommand(ShowInfoMenuView);
            SignOutCommand = new ExecuteMethodCommand(SignOut);
        }

        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }


        private void ShowTourTrackingView()
        {
            CurrentKeyPointViewModel currentKeyPointViewModel = new CurrentKeyPointViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, currentKeyPointViewModel ));

            navigate.Execute(null);
        }

        private void ShowMyVouchersView()
        {
            MyVouchersViewModel myVouchersViewModel = new MyVouchersViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, myVouchersViewModel));

            navigate.Execute(null);
        }

        private void ShowTourBrowserView()
        {
            NewTourBrowserViewModel newTourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, newTourBrowserViewModel));

            navigate.Execute(null);
        }

        private void ShowRequestMenuView()
        {
            Guest2RequestMenuViewModel requestMenuViewModel = new Guest2RequestMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, requestMenuViewModel));

            navigate.Execute(null);
        }
        private void ShowInfoMenuView()
        {
            Guest2InfoMenuViewModel infoMenuViewModel = new Guest2InfoMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, infoMenuViewModel));

            navigate.Execute(null);
        }
    }
}
