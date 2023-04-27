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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class OwnerCommentViewModel : ViewModelBase
    {
        private readonly User _user;
        public string Comment { get; set; }
        private readonly NavigationStore _navigationStore;
        public ICommand NavigateMyRequestsCommand { get; }
        public OwnerCommentViewModel(NavigationStore navigationStore, User user, string comment)
        {
            _navigationStore = navigationStore;
            _user = user;
            Comment = comment;
            NavigateMyRequestsCommand = new ExecuteMethodCommand(NavigateMyRequests);
        }
        private void NavigateMyRequests()
        {
            var contentViewModel = new MyAccommodationReservationRequestsViewModel(_navigationStore, _user);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
    }
}
