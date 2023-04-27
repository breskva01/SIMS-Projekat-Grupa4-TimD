using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using System.Windows.Input;
using InitialProject.Application.Commands;

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class AccommodationReservationViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly NavigationStore _navigationStore;
        public Accommodation Accommodation { get; set; }
        public User Guest { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICommand FindAvailableReservationsCommand { get; }
        public ICommand NavigateAccommodationBrowserCommand { get; }
        public AccommodationReservationViewModel(NavigationStore navigationStore ,User user, Accommodation accommodation)
        {
            _navigationStore = navigationStore;
            Guest = user;
            Accommodation = accommodation;
            _reservationService = new AccommodationReservationService();
            FindAvailableReservationsCommand = new ExecuteMethodCommand(GetAvailableReservations);
            NavigateAccommodationBrowserCommand = new ExecuteMethodCommand(NavigateAcoommodationBrowser);
        }

        private void GetAvailableReservations()
        {
            if (Days == 0)
                MessageBox.Show("Unesite željeni broj dana.");
            else if (Days < Accommodation.MinimumDays)
                MessageBox.Show($"Minimalani broj dana: {Accommodation.MinimumDays}");
            else if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
            {
                DateOnly startDate = DateOnly.FromDateTime(StartDate);
                DateOnly endDate = DateOnly.FromDateTime(EndDate);
                List<AccommodationReservation> reservations = _reservationService.GetAvailable(startDate, endDate, Days, Accommodation, Guest);
                ShowDatePicker(reservations);
                
            }
            else
                MessageBox.Show("Izaberite željeni opseg datuma");
        }
        private void ShowDatePicker(List<AccommodationReservation> reservations)
        {
            var viewModel = new AccommodationReservationDatePickerViewModel(_navigationStore, reservations);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
        private void NavigateAcoommodationBrowser()
        {
            var contentViewModel = new AccommodationBrowserViewModel(_navigationStore, Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
        }
    }
}
