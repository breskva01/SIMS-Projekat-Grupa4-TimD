using InitialProject.Application.Commands;
using InitialProject.Application.Factories;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class ReceivedRatingsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public List<GuestRating> Ratings { get; set; }
        private readonly GuestRatingService _ratingService;
        public ICommand ShowRatingCommand { get; }
        public ReceivedRatingsViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _ratingService = new GuestRatingService();
            Ratings = _ratingService.GetEligibleForDisplay(_user.Id);
            ShowRatingCommand = new GuestRatingClickCommand(NavigateRatingDetails);
        }
        private void NavigateRatingDetails(GuestRating rating)
        {
            var viewModel = ViewModelFactory.Instance.CreateGuestRatingDetailsVM(_navigationStore, _user, rating);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
