using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerRatingsViewModel: ViewModelBase
    {
        public ObservableCollection<AccommodationRating> OwnerRatings { get; set; }
        private AccommodationRatingService _accommodatonRatingService;
        private readonly User _owner;

        public OwnerRatingsViewModel(NavigationStore navigationStore, User user)
        {
            _owner = user;
            _accommodatonRatingService= new AccommodationRatingService();
            OwnerRatings = new ObservableCollection<AccommodationRating>(_accommodatonRatingService.GetEgligibleForDisplay(_owner.Id));
        }
    }
}
