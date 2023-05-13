using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
            SetAppStatus();
        }

        private void SetAppStatus()
        {
            if (ContentViewModel is AccommodationBrowserViewModel)
            {
                NavigationBarViewModel.SelectedTab = "AccommodationBrowser";
                return;
            }
            if (ContentViewModel is MyAccommodationReservationsViewModel)
            {
                NavigationBarViewModel.SelectedTab = "MyReservations";
                return;
            }
            if (ContentViewModel is MyAccommodationReservationRequestsViewModel)
            {
                NavigationBarViewModel.SelectedTab = "MyRequests";
                return;
            }
            if (ContentViewModel is AccommodationRatingViewModel)
            {
                NavigationBarViewModel.SelectedTab = "Ratings";
                return;
            }
        }
    }
}
