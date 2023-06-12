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
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class GuestRatingDetailsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public GuestRating Rating { get; set; }
        public ICommand NavigateRatingsCommand { get; }
        public GuestRatingDetailsViewModel(NavigationStore navigationStore, Guest1 user, GuestRating rating)
        {
            _navigationStore = navigationStore;
            _user = user;
            Rating = rating;
            NavigateRatingsCommand = new ExecuteMethodCommand(NavigateRatings);
        }
        private void NavigateRatings()
        {
            var viewModel = ViewModelFactory.Instance.CreateRatingsVM(_navigationStore, _user, 1);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
