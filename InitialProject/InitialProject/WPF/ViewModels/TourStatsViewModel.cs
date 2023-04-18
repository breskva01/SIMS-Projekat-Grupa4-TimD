using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class TourStatsViewModel : ViewModelBase
    {
        public ObservableCollection<Location> Locations { get; set; }
        private readonly NavigationStore _navigationStore;
        private User _user;

        private LocationService _locationService;

        private Tour tour;

        private readonly ObservableCollection<Tour> _tours;
        public IEnumerable<Tour> MostVisited => _tours;

        private TourService _tourService;

        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                ComboBoxSelectionChanged();

            }
        }

        private List<string> _avaiableYears;
        public List<string> AvaiableYears
        {
            get { return _avaiableYears; }
            set
            {
                _avaiableYears = value;
                OnPropertyChanged(nameof(AvaiableYears));
            }
        }

        private void ComboBoxSelectionChanged()
        {
            _tours.Clear();
            tour = _tourService.GetMostVisited(SelectedYear);
            foreach (Tour t in _tours)
            {
                foreach (Location l in Locations)
                {
                    if (t.LocationId == l.Id)
                        t.Location = l;
                }
            }
            _tours.Add(tour);
        }


    public TourStatsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tours = new ObservableCollection<Tour>();

            _tourService = new TourService();
            _locationService = new LocationService();

            tour = new Tour();
            _avaiableYears = new List<string>();
            _avaiableYears = _tourService.GetAvailableYears();
            _avaiableYears.Add("All time");

            Locations = new ObservableCollection<Location>(_locationService.GetAll());


            
        }
    }
}
