using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class AllToursViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursShow;
        public IEnumerable<Tour> ToursShow => _toursShow;

        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private User _user;

        private LocationService _locationService;
        private TourService _tourService;

        public ICommand CancelTourCommand { get; set; }

        private DateTime _today;

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));

            }
        }

        public AllToursViewModel(NavigationStore navigationStore, User user)
        {
            _toursShow = new ObservableCollection<Tour>();

            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _locationService = new LocationService();
            _tours = new ObservableCollection<Tour>(_tourService.GetAll());
            Locations = new ObservableCollection<Location>(_locationService.GetAll());

            foreach (Tour t in _tours)
            {
                foreach (Location l in Locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            foreach (Tour t in _tours)
            {
                if (t.State == 0)
                {
                    _toursShow.Add(t);
                }
            }

            _today = DateTime.Now;

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            CancelTourCommand = new ExecuteMethodCommand(CancelTour);
        }
        private void CancelTour()
        {
            if (SelectedTour == null)
            {
                MessageBox.Show("Please select a tour first.");

                return;
            }
            if (SelectedTour.Start > _today.AddHours(48))
            {
                if (SelectedTour.State == TourState.None)
                {
                    SelectedTour.State = TourState.Canceled;
                    _tourService.Update(SelectedTour);

                    _toursShow.Remove(SelectedTour);
                    return;
                }
            }
            MessageBox.Show("It's too late to cancel this tour.");
            return;
        }
    }
}
