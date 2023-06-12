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
    public class AccommodationReservationMoveRequestViewModel : ViewModelBase
    {
        private readonly AccommodationReservationRequestService _requestService;
        private readonly NavigationStore _navigationStore;
        public AccommodationReservation Reservation { get; set; }
        private DateTime _checkIn;
        public DateTime CheckIn
        {
            get => _checkIn;
            set
            {
                if (_checkIn != value)
                {
                    _checkIn = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime CheckOut { get; set; }
        public ICommand SubmitRequestCommand { get; }
        public ICommand NavigateMyResevationsCommand { get; }
        public AccommodationReservationMoveRequestViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            CheckIn = DateTime.Now;
            _navigationStore = navigationStore;
            _requestService = new AccommodationReservationRequestService();
            Reservation = reservation;
            SubmitRequestCommand = new ExecuteMethodCommand(SubmitRequest);
            NavigateMyResevationsCommand = new ExecuteMethodCommand(NavigateMyReservations);
        }
        private void SubmitRequest()
        {
            if (CheckIn != DateTime.MinValue && CheckOut != DateTime.MinValue)
            {
                DateOnly checkIn = DateOnly.FromDateTime(CheckIn);
                DateOnly checkOut = DateOnly.FromDateTime(CheckOut);
                _requestService.Save(Reservation, checkIn, checkOut);
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Zahtev uspešno poslat.");
                else
                    MessageBox.Show("Request succesffuly sent.");
                NavigateMyReservations();
            }
            else if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                MessageBox.Show("Izaberite željene datume.");
            else
                MessageBox.Show("Please choose the desire check in & check out dates.");
        }
        private void NavigateMyReservations()
        {
            var viewModel = ViewModelFactory.Instance.CreateMyReservationsVM(_navigationStore, Reservation.Guest);
            NavigationService.Instance.Navigate(viewModel);
        }
    }
}
