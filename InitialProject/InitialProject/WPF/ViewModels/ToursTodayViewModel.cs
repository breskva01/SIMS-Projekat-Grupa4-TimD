using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ToursTodayViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Tour> _tours;
        private readonly ObservableCollection<Tour> _toursToday;
        public IEnumerable<Tour> ToursToday => _toursToday;

        public ObservableCollection<Location> Locations { get; set; }
        private LocationService _locationService;

        private DateTime _today;

        public int NumberOfActiveTours;

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

        private TourService _tourService;

        private readonly NavigationStore _navigationStore;
        private User _user;

        public ICommand StartTourCommand { get; set; }

        public ToursTodayViewModel(NavigationStore navigationStore, User user) 
        {
            _today = DateTime.Now;
            _toursToday = new ObservableCollection<Tour>();

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
                if (IsDateToday(t))
                {
                    _toursToday.Add(t);
                }
            }

            NumberOfActiveTours = 0;

            InitializeCommands();

        }
        private void InitializeCommands()
        {
            StartTourCommand = new ExecuteMethodCommand(StartTour);
        }
        private bool IsDateToday(Tour t)
        {
            return t.Start.Year == _today.Year && t.Start.Month == _today.Month && t.Start.Day == _today.Day;
        }

        public ICommand LiveTrackingNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, LiveTracking()));

        private void StartTour()
        {
            if(SelectedTour != null)
            {
                if(NumberOfActiveTours != 0)
                {
                    return;
                }
                if(SelectedTour.State == (TourState)TourState.None || SelectedTour.State == (TourState)TourState.Started)
                {
                    SelectedTour.State = (TourState)TourState.Started;
                    _tourService.Update(SelectedTour);
                    NumberOfActiveTours++;
                    LiveTrackingNavigateCommand.Execute(null);
                    SelectedTour = null;
                    return;
                }
            }
            return;
        }
        private TourLiveTrackingViewModel LiveTracking()
        {
            return new TourLiveTrackingViewModel(_navigationStore, _user, SelectedTour);

        }


    }
}
