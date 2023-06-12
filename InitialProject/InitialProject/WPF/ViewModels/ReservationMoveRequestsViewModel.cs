using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ReservationMoveRequestsViewModel: ViewModelBase
    {
        public ObservableCollection<AccommodationReservationMoveRequest> MoveRequests { get; set; }
        
        private readonly AccommodationReservationService _reservationService;
        private readonly User _owner;
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public bool IsNotified;

        private AccommodationReservationMoveRequest _selectedRequest { get; set; }
        public AccommodationReservationMoveRequest SelectedRequest 
        {
            get => _selectedRequest;
            set
            {
                if (value != _selectedRequest)
                {
                    _selectedRequest = value;
                    OnPropertyChanged();
                    if(_selectedRequest != null)
                    Availability = CheckAvailability(_selectedRequest.Reservation.Accommodation.Id, _selectedRequest.RequestedCheckIn, _selectedRequest.RequestedCheckOut);
                }
            }
        }

        private string _availability;
        public string Availability
        {
            get => _availability;
            set
            {
                if (value != _availability)
                {
                    _availability = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackNavigateCommand =>
        new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        public ReservationMoveRequestsViewModel(NavigationStore navigationStore, User user, bool isNotified) 
        {
            _owner = user;
            IsNotified = isNotified;
            _requestService = new AccommodationReservationRequestService();
            _reservationService= new AccommodationReservationService();
            _navigationStore = navigationStore;
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequest> (_requestService.GetPendingRequestsByOwnerId(_owner.Id));
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            ApproveCommand = new ExecuteMethodCommand(Approve);
            DenyCommand = new ExecuteMethodCommand(Deny);
            BackCommand = new ExecuteMethodCommand(Back);
        }

        private void Approve()
        {
            if (SelectedRequest == null)
            {
                MessageBox.Show("You have to select a request!");
            }
            else
            {
                _reservationService.MoveReservation(SelectedRequest.Reservation.Id, SelectedRequest.RequestedCheckIn, SelectedRequest.RequestedCheckOut);
                _requestService.ApproveRequest(SelectedRequest.Reservation.Id);
                MoveRequests.Remove(SelectedRequest);
            }
        }
        private void Deny()
        {
            if (SelectedRequest == null)
            {
                MessageBox.Show("You have to select a request!");
            }
            else
            {
                RequestDeniedView requestDeniedView = new RequestDeniedView(SelectedRequest);
                requestDeniedView.Show();
            }
        }
        private string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut)
        {
            return _reservationService.CheckAvailability(accommodationId, checkIn, checkOut);
        }
        private void Back()
        {
            BackNavigateCommand.Execute(null);
        }
        private AccommodationsViewModel GoBack()
        {
            return new AccommodationsViewModel(_navigationStore, _owner, IsNotified);
        }
    }
}
