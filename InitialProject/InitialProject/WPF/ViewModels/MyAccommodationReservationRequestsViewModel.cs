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

namespace InitialProject.WPF.ViewModels
{
    public class MyAccommodationReservationRequestsViewModel : ViewModelBase
    {
        private readonly User _loggedInUser;
        public List<AccommodationReservationMoveRequest> Requests { get; set; }
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public ICommand ShowAccommodationBrowserViewCommand { get; }
        public ICommand ShowCommentViewCommand { get; }
        public MyAccommodationReservationRequestsViewModel(NavigationStore navigationStore, User loggedInUser)
        {
            _navigationStore = navigationStore;
            _loggedInUser = loggedInUser;
            _requestService = new AccommodationReservationRequestService();
            Requests = _requestService.GetByGuestId(_loggedInUser.Id);
            ShowAccommodationBrowserViewCommand = new ExecuteMethodCommand(ShowAccommodationBrowserView);
            ShowCommentViewCommand = new AccommodationReservationRequestClickCommand(ShowComment);
        }
        private void ShowAccommodationBrowserView()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
        private void ShowComment(AccommodationReservationMoveRequest request)
        {
            if (string.IsNullOrEmpty(request.Comment))
                MessageBox.Show("Vlasnik nije ostavio komentar.");
            else
            {
                var viewModel = new OwnerCommentViewModel(_navigationStore, _loggedInUser, request);
                var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
                navigateCommand.Execute(null);
            }
        }
    }
}
