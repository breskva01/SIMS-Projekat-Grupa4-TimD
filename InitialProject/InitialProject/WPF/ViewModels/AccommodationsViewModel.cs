using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InitialProject.Application.Services;
using System.Collections.ObjectModel;
using InitialProject.WPF.NewViews.Owner;
using InitialProject.WPF.NewViews;
using System.Security.RightsManagement;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationsViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly AccommodationService _accommodationService;
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        private bool _isNotified;
        public bool IsNotified
        {
            get => _isNotified;
            set
            {
                if (value != _isNotified)
                {
                    _isNotified = value;
                    OnPropertyChanged(nameof(IsNotified));
                }
            }

        }
        public ICommand BackCommand { get; }
        public ICommand ShowAccommodationStatisticsViewCommand { get; }
        public ICommand ShowScheduleRenovationViewCommand { get; }
        public ICommand ShowMyRenovationAppointmentsViewCommand { get; }
        public ICommand ShowAccommodationRegistrationViewCommand { get; }
        public ICommand ShowReservationMoveRequestsViewCommand { get; }
        public AccommodationsViewModel(NavigationStore navigationStore, User user, bool isNotified)
        {
            _navigationStore = navigationStore;
            _user = user;
            IsNotified = isNotified;
            _accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAllOwnersAccommodations(_user.Id));
            BackCommand = new ExecuteMethodCommand(Back);
            ShowAccommodationStatisticsViewCommand = new ExecuteMethodCommand(ShowAccommodationStatisticsView);
            ShowScheduleRenovationViewCommand = new ExecuteMethodCommand(ShowScheduleRenovationView);
            ShowMyRenovationAppointmentsViewCommand = new ExecuteMethodCommand(ShowMyRenovationAppointmentsView);
            ShowAccommodationRegistrationViewCommand = new ExecuteMethodCommand(ShowAccommodationRegistrationView);
            ShowReservationMoveRequestsViewCommand = new ExecuteMethodCommand(ShowReservationMoveRequestsView);
        }
        private void Back()
        {
            OwnerProfileViewModel ownerProfileViewModel = new OwnerProfileViewModel(_navigationStore, (Owner)_user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, ownerProfileViewModel));

            navigate.Execute(null);
        }
        private void ShowAccommodationStatisticsView()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("You have to select an acommodation!");
            }
            else
            {
                AccommodationStatisticsView accommodationStatisticsView = new AccommodationStatisticsView(SelectedAccommodation, (Owner)_user);
                accommodationStatisticsView.Show();
            }
        }
        private void ShowScheduleRenovationView()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("You have to select an acommodation!");
            }
            else
            {
                ScheduleRenovationView scheduleRenovationView = new ScheduleRenovationView(SelectedAccommodation);
                scheduleRenovationView.Show();
            }
        }
        private void ShowMyRenovationAppointmentsView()
        {
            MyRenovationAppointmentsView myRenovationAppointmentsView = new MyRenovationAppointmentsView(_user.Id);
            myRenovationAppointmentsView.Show();
        }
        private void ShowAccommodationRegistrationView()
        {
            AccommodationRegistrationView accommodationRegistrationView = new AccommodationRegistrationView(_user);
            accommodationRegistrationView.Show();
        }
        private void ShowReservationMoveRequestsView()
        {
            ReservationMoveRequestsViewModel reservationMoveRequestsViewModel = new ReservationMoveRequestsViewModel(_navigationStore, _user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, reservationMoveRequestsViewModel));

            navigate.Execute(null);
        }
    }
}
