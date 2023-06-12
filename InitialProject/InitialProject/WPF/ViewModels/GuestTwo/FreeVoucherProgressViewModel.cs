using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class FreeVoucherProgressViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private Guest2 _guest;
        public int FreeVoucherProgress { get; set; }

        public ICommand MenuCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NotificationCommand { get; }

        public FreeVoucherProgressViewModel(NavigationStore navigationStore, Guest2 guest)
        {
            _navigationStore = navigationStore;
            _guest = guest;

            FreeVoucherProgress = guest.FreeVoucherProgress;
            


            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            BackCommand = new ExecuteMethodCommand(ShowVoucherBrowserView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
        }

        private void ShowVoucherBrowserView()
        {
            MyVouchersViewModel myVouchersViewModel = new MyVouchersViewModel(_navigationStore, _guest);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, myVouchersViewModel));
            navigate.Execute(null);
        }


        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _guest);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _guest);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }
    }
}
