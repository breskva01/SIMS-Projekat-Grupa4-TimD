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
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class ComplexTourRequestBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<ComplexTourRequest> ComplexTourRequests { get; set; }
        public Tour SelectedComplexTourRequest { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly LocationService _locationService;

        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }
        public ICommand ShowComplexRequestCommand { get; }


        public ComplexTourRequestBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourRequestService = new TourRequestService();
            _complexTourRequestService = new ComplexTourRequestService();
            _locationService = new LocationService();

            ComplexTourRequests = new ObservableCollection<ComplexTourRequest>();
            foreach (var t in _complexTourRequestService.GetByUser(_user.Id))
            {
                ComplexTourRequests.Add(t);
            }

            BackCommand = new ExecuteMethodCommand(ShowGuest2InfoMenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationsView);
            ShowComplexRequestCommand = new ComplexRequestClickCommand(ShowComplexRequestDetailsView);
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


        private void ShowNotificationsView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));

            navigate.Execute(null);
        }

        private void ShowComplexRequestDetailsView(ComplexTourRequest selectedRequest)
        {
            ComplexTourRequestDetailsViewModel complexTourRequestDetailsViewModel = new ComplexTourRequestDetailsViewModel(_navigationStore, _user, selectedRequest);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, complexTourRequestDetailsViewModel));
            navigate.Execute(null);
        }
    }
}
