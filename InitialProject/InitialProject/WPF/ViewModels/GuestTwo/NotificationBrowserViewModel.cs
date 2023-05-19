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
using System.Windows;
using InitialProject.Repositories.FileHandlers;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class NotificationBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<UserNotification> Notifications { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly UserNotificationService _notificationService;

        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }


        public NotificationBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _notificationService = new UserNotificationService();

            Notifications = new ObservableCollection<UserNotification>();
            foreach (var t in _notificationService.GetByUser(_user.Id))
            {
                Notifications.Add(t);
            }

            BackCommand = new ExecuteMethodCommand(ShowGuest2InfoMenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowGuest2InfoMenuView()
        {
            Guest2InfoMenuViewModel guest2InfoMenuViewModel = new Guest2InfoMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2InfoMenuViewModel));
            navigate.Execute(null);
        }

    }
}
