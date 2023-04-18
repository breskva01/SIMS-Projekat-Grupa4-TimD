using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class RateAccommodationViewModel : ViewModelBase
    {
        public AccommodationReservation Reservation { get; set; }
        private readonly AccommodationReservationService _service;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }
        public ICommand ShowAccommodationRatingsCommand { get; }
        public int LocationRating { get; set; }
        public int HygeneRating { get; set; }
        public int PleasantnessRating { get; set; }
        public int FairnessRating { get; set; }
        public int ParkingRating { get; set; }
        public string Comment { get; set; }
        public RateAccommodationViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            _navigationStore = navigationStore;
            Reservation = reservation;
            _service = new AccommodationReservationService();      
        }
    }
}
