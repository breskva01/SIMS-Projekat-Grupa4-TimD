using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
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
        private User _user;
        public Tour _tour;
        private readonly NavigationStore _navigationStore;
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

        public TourReservationViewModel(NavigationStore navigationStore, User user, Tour tour)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tour = tour;
            //ReserveCommand = new MakeReservationCommand(this, tourBrowserNavigationService);
            //CancelCommand = new NavigateCommand(tourBrowserNavigationService);
        }

    }
}
