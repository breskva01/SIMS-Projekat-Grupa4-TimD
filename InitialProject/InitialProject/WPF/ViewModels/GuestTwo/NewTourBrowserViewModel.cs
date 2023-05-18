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
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using InitialProject.WPF.ViewModels.GuestTwo;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class NewTourBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTour { get; set; }

        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;

        private TourFilterSort _tourFilterSort;


        public ICommand FilterCommand { get; }
        //public ICommand ResetCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand MakeReservationCommand { get; }
        public ICommand MenuCommand { get; }


        public NewTourBrowserViewModel(NavigationStore navigationStore, User user, TourFilterSort tourFilterSort = null)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _locationService = new LocationService();
            _tourFilterSort = tourFilterSort ?? new TourFilterSort();

            Tours = new ObservableCollection<Tour>();
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            foreach (var t in _tourService.GetFiltered(_tourFilterSort)) 
            {
                t.Location = Locations.FirstOrDefault(l => l.Id == t.LocationId);
                Tours.Add(t);
            }

            var sortedTours = _tourService.GetSorted(new List<Tour>(Tours), _tourFilterSort);
            Tours.Clear();
            foreach (var tour in sortedTours)
            {
                Tours.Add(tour);
                tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
            }
            //Tours = _tourService.GetSorted(Tours, tourFilterSort);

            FilterCommand = new ExecuteMethodCommand(ShowTourFilterView);
            //ResetCommand = new ExecuteMethodCommand(ResetFilter);
            SortCommand = new ExecuteMethodCommand(ShowTourSortView);
            MakeReservationCommand = new TourClickCommand(ShowTourReservationView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowTourReservationView(Tour tour)
        {
            if (tour.CurrentNumberOfGuests == tour.MaximumGuests)
            {
                MessageBox.Show("Unfortunately, the tour that you are interested in is fully booked. " +
                    "On the previous window you can take a look at other tours that are located in the same location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                
                _tourFilterSort.FilterCountry = tour.Location.Country;
                _tourFilterSort.FilterCity = tour.Location.City;
                _tourFilterSort.FilterMinDuration = 0;
                _tourFilterSort.FilterMaxDuration = 0;
                _tourFilterSort.FilterLanguage = GuideLanguage.All;
                _tourFilterSort.FilterNumberOfGuests = 1;
                Tours.Clear();
                foreach (var t in _tourService.GetFiltered(_tourFilterSort))
                {
                    t.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                    Tours.Add(t);
                }

                return;
            }

            var viewModel = new TourReservationViewModel(_navigationStore, _user, tour);
            var reserveTourNavigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            reserveTourNavigateCommand.Execute(null);
        }

        private void ShowTourFilterView()
        {
            TourFilterViewModel tourFilterViewModel = new TourFilterViewModel(_navigationStore, _user, _tourFilterSort);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourFilterViewModel));
            navigate.Execute(null);
        }

        private void ShowTourSortView()
        {
            TourSortViewModel tourSortViewModel = new TourSortViewModel(_navigationStore, _user, _tourFilterSort);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourSortViewModel));
            navigate.Execute(null);
        }


        /*
        
        private void ResetFilter()
        {
            SelectedCountry = "";
            SelectedCity = "";
            Duration = "0";
            SelectedLanguageIndex = 0;
            NumberOfGuests = "0";

            Tours.Clear();
            foreach (var tour in _tourService.GetAll())
            {
                tour.Location = Locations.FirstOrDefault(l => l.Id == tour.LocationId);
                Tours.Add(tour);
            }
        }
         */

           

    }
}

