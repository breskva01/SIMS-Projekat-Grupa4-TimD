using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerRatingsViewModel: ViewModelBase
    {
        public ObservableCollection<AccommodationRating> OwnerRatings { get; set; }
        private AccommodationRatingService _accommodatonRatingService;
        private readonly User _owner;
        private readonly NavigationStore _navigationStore;
        public OwnerRatingsViewModel(NavigationStore navigationStore, User user)
        {
            _owner = user;
            _navigationStore = navigationStore;
            _accommodatonRatingService= new AccommodationRatingService();
            OwnerRatings = new ObservableCollection<AccommodationRating>(_accommodatonRatingService.GetEligibleForDisplay(_owner.Id));
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            BackCommand = new ExecuteMethodCommand(Back);
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
