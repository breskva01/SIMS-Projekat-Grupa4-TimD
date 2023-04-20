using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
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

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

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
                    Availability = CheckAvailability(_selectedRequest.Reservation.AccommodationId, _selectedRequest.RequestedCheckIn, _selectedRequest.RequestedCheckOut);
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

        public ReservationMoveRequestsViewModel(NavigationStore navigationStore, User user) 
        {
            _owner = user;
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
            _reservationService.MoveReservation(SelectedRequest.ReservationId, SelectedRequest.RequestedCheckIn, SelectedRequest.RequestedCheckOut);
            _requestService.ApproveRequest(SelectedRequest.ReservationId);
            MoveRequests.Remove(SelectedRequest);
        }
        private void Deny()
        {
            _requestService.DenyRequest(SelectedRequest.ReservationId, Comment);
            Comment = "";
            MoveRequests.Remove(SelectedRequest);
        }
        private string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut)
        {
            return _reservationService.CheckAvailability(accommodationId, checkIn, checkOut);
        }
        private void Back()
        {
            BackNavigateCommand.Execute(null);
        }
        private OwnerViewModel GoBack()
        {
            return new OwnerViewModel(_navigationStore, _owner);
        }
    }
}
