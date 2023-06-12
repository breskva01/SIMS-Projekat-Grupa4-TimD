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
    public class ComplexTourRequestDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public ObservableCollection<Location> Locations { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;

        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }


        public ComplexTourRequestDetailsViewModel(NavigationStore navigationStore, User user, ComplexTourRequest selectedRequest)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();

            TourRequests = new ObservableCollection<TourRequest>();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            int i = 1;
            foreach (var t in selectedRequest.TourRequests)
            {
                t.Location = Locations.FirstOrDefault(l => l.Id == t.Location.Id);
                t.OrderNumber = i;
                TourRequests.Add(t);
                i++;
            }

            BackCommand = new ExecuteMethodCommand(ShowComplexTourRequestBrowserView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }
        private void ShowComplexTourRequestBrowserView()
        {
            ComplexTourRequestBrowserViewModel complexTourRequestBrowserViewModel = new ComplexTourRequestBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, complexTourRequestBrowserViewModel));
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
