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

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourRequestBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTourRequest { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;

        public ICommand FilterCommand { get; }
     
        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }


        public TourRequestBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            TourRequests = new ObservableCollection<TourRequest>();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            foreach (var t in _tourRequestService.GetByUser(_user.Id))
            {
                t.Location = Locations.FirstOrDefault(l => l.Id == t.Location.Id);
                TourRequests.Add(t);
            }

            FilterCommand = new ExecuteMethodCommand(ShowApprovedTourRequestView);
            BackCommand = new ExecuteMethodCommand(ShowGuest2InfoMenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
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

        private void ShowApprovedTourRequestView()
        {
            ApprovedTourRequestsViewModel approvedTourRequestsViewModel = new ApprovedTourRequestsViewModel(_navigationStore, _user, TourRequests.ToList());
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, approvedTourRequestsViewModel));
            navigate.Execute(null);
        }

        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }

    }
}
