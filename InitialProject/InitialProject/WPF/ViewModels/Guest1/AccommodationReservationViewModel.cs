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
        public string SecondImagePath => Accommodation.PictureURLs.Count > 1 ? Accommodation.PictureURLs[1] : null;
        public string ThirdImagePath => Accommodation.PictureURLs.Count > 2 ? Accommodation.PictureURLs[2] : null;
        public string FourthImagePath => Accommodation.PictureURLs.Count > 3 ? Accommodation.PictureURLs[3] : null;
        public string FifthImagePath => Accommodation.PictureURLs.Count > 4 ? Accommodation.PictureURLs[4] : null;

        public User Guest { get; set; }
        public int Days { get; set; }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime EndDate { get; set; }
        public ICommand NavigateImageBrowserCommand { get; }
        public ICommand FindAvailableReservationsCommand { get; }
        public ICommand NavigateAccommodationBrowserCommand { get; }
        public AccommodationReservationViewModel(NavigationStore navigationStore ,User user, Accommodation accommodation)
        {
            StartDate = DateTime.Now;
            _navigationStore = navigationStore;
            Guest = user;
            Accommodation = accommodation;
            _reservationService = new AccommodationReservationService();
            FindAvailableReservationsCommand = new ExecuteMethodCommand(GetAvailableReservations);
            NavigateAccommodationBrowserCommand = new ExecuteMethodCommand(NavigateAcoommodationBrowser);
            NavigateImageBrowserCommand = new ImageClickCommand(NavigateImageBrowser);
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
        private void NavigateImageBrowser(string imageURL)
        {
            var viewModel = new ImageBrowserViewModel(_navigationStore, Guest, Accommodation, imageURL);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }
}
