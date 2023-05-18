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

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class GuestRatingDetailsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public GuestRating Rating { get; set; }
        public ICommand NavigateRatingsCommand { get; }
        public GuestRatingDetailsViewModel(NavigationStore navigationStore, Guest1 user,
                                           GuestRating rating)
        {
            _navigationStore = navigationStore;
            _user = user;
            Rating = rating;
            NavigateRatingsCommand = new ExecuteMethodCommand(NavigateRatings);
        }
        private void NavigateRatings()
        {
            var contentViewModel = new AccommodationRatingViewModel(_navigationStore, _user, 1);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
    }
}
