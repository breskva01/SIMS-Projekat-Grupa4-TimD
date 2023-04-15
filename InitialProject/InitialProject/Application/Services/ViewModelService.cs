using InitialProject.Application.Stores;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class ViewModelService
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelService(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
        }
        public TourReservationViewModel CreateTourReservationViewModel()
        {
            return new TourReservationViewModel(new NavigationService(_navigationStore, CreateTourBrowserViewModel));
        }

        public TourBrowserViewModel CreateTourBrowserViewModel()
        {
            return new TourBrowserViewModel(new NavigationService(_navigationStore, CreateTourReservationViewModel));
        }
        /*
        public AccommodationBrowserViewModel CreateAccommodationBrowserViewModel()
        {
            return new AccommodationBrowserViewModel(new NavigationService(_navigationStore, CreateAccommodationReservationViewModel));
        }

        public AccommodationReservationViewModel CreateAccommodationReservationViewModel()
        {
            return new AccommodationReservationViewModel(new NavigationService(_navigationStore, CreateAccommodationBrowserViewModel));
        }
        */

        public SignInViewModel CreateSignInViewModel()
        {
            return new SignInViewModel(_navigationStore, CreateTourBrowserViewModel);
        }
    }
}
