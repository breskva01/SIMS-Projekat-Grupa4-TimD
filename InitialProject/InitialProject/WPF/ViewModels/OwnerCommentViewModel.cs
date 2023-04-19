using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerCommentViewModel : ViewModelBase
    {
        private readonly User _loggedInUser;
        public AccommodationReservationMoveRequest Request { get; set; }
        private readonly NavigationStore _navigationStore;
        public ICommand ShowRequestsViewCommand { get; }
        public OwnerCommentViewModel(NavigationStore navigationStore, User loggedInUser, AccommodationReservationMoveRequest request)
        {
            _navigationStore = navigationStore;
            _loggedInUser = loggedInUser;
            Request = request;
            ShowRequestsViewCommand = new ExecuteMethodCommand(ShowRequestsView);
        }
        private void ShowRequestsView()
        {
            var viewModel = new MyAccommodationReservationRequestsViewModel(_navigationStore, _loggedInUser);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
