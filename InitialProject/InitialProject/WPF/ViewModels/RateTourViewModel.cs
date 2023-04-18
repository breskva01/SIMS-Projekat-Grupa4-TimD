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
    public class RateTourViewModel : ViewModelBase
    {
        private User _user;
        public Tour Tour { get; set; }
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly NavigationStore _navigationStore;

        public int KnowledgeRating { get; set; }
        public int LanguageRating { get; set; }
        public int InterestingRating { get; set; }
        public int InformativeRating { get; set; }
        public int ContentRating { get; set; }
        public string Comment { get; set; }

        public ICommand BackCommand { get; }
        public ICommand RateCommand { get; }

        public RateTourViewModel(NavigationStore navigationStore, User user, Tour tour) {
            _navigationStore = navigationStore;
            _user = user;
            Tour = tour;
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();

            //BackCommand = new ExecuteMethodCommand(ShowTourRatingView);
            //RateCommand = new ExecuteMethodCommand(RateTour)

            
            
        }
    }
}
