using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class RequestDeniedViewModel : ViewModelBase
    {
        private readonly AccommodationReservationRequestService _accommodationReservationRequestService;
        private AccommodationReservationMoveRequest _selectedRequest;
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
        public ICommand ConfirmCommand { get; }

        public RequestDeniedViewModel(AccommodationReservationMoveRequest selectedRequest) 
        {
            _selectedRequest = selectedRequest;
            _accommodationReservationRequestService = new AccommodationReservationRequestService();
            ConfirmCommand = new ExecuteMethodCommand(Confirm);
        }
        private void Confirm()
        {
            if(Comment == "" || Comment == null)
            {
                MessageBox.Show("You have to give an elaboration for the denial!");
            }
            else
            _accommodationReservationRequestService.DenyRequest(_selectedRequest.Reservation.Id, Comment);
        }
    }
}
