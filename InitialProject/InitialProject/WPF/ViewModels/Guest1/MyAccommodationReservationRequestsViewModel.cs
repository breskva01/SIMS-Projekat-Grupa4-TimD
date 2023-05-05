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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class MyAccommodationReservationRequestsViewModel : ViewModelBase
    {
        private readonly User _user;
        public List<AccommodationReservationMoveRequest> Requests { get; set; }
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public ICommand ShowCommentCommand { get; }
        public MyAccommodationReservationRequestsViewModel(NavigationStore navigationStore, User loggedInUser)
        {
            _navigationStore = navigationStore;
            _user = loggedInUser;
            _requestService = new AccommodationReservationRequestService();
            Requests = _requestService.GetByGuestId(_user.Id);
            ShowCommentCommand = new AccommodationReservationRequestClickCommand(ShowComment);
        }
        private void ShowComment(AccommodationReservationMoveRequest request)
        {
            if (string.IsNullOrEmpty(request.Comment))
                MessageBox.Show("Vlasnik nije ostavio komentar.");
            else
            {
                var viewModel = new OwnerCommentViewModel(_navigationStore, _user, request.Comment);
                var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
                navigateCommand.Execute(null);
            }
        }
    }
}
