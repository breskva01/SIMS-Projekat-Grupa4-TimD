using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourReservationViewModel : ViewModelBase
    {

        private string _numberOfGuests;
        public  string NumberOfGuests
        {
            get
            { 
                return _numberOfGuests;
            }
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged(nameof(NumberOfGuests));
            }
        }
      
        public ICommand ReserveCommand { get; }
        public ICommand CancelCommand { get; }

        public TourReservationViewModel(NavigationService tourBrowserNavigationService)
        {
            ReserveCommand = new MakeReservationCommand(this, tourBrowserNavigationService);
            CancelCommand = new NavigateCommand(tourBrowserNavigationService);
        }

    }
}
