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
    public class ApprovedTourRequestsViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private List<TourRequest> _tourRequests;
        private List<Location> _locations;
        private TourRequestService _tourRequestService;
        private LocationService _locationService;

        //public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<ApprovedRequestViewModel> ApprovedRequests { get; set; }

        public ICommand MenuCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NotificationCommand { get; }

        public ApprovedTourRequestsViewModel(NavigationStore navigationStore, User user, List<TourRequest> tourRequests)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            _locations = new List<Location>(_locationService.GetAll());
            
            _tourRequests = _tourRequestService.GetApproved(tourRequests);

            ApprovedRequests = new ObservableCollection<ApprovedRequestViewModel>();
            
            foreach (var t in _tourRequests)
            {
                ApprovedRequests.Add(new ApprovedRequestViewModel(t));
            }

            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            BackCommand = new ExecuteMethodCommand(ShowTourRequestBrowserView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
        }

        private void ShowTourRequestBrowserView()
        {
            TourRequestBrowserViewModel tourRequestBrowserViewModel = new TourRequestBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestBrowserViewModel));
            navigate.Execute(null);
        }


        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
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
