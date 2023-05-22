using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourRequestStats2ViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;
        private TourRequestService _tourRequestService;
        private LocationService _locationService;

        public List<string> Locations { get; set; }
        public List<int> LocationNumberOfRequests { get; set; }
        public List<string> GuideLanguages { get; set; }
        public List<int> LanguageNumberOfRequests { get; set; }
        public SeriesCollection SeriesCollectionLocation { get; set; }
        public SeriesCollection SeriesCollectionLanguage { get; set; }
        public string[] LocationsAxes { get; set; }
        public string[] LanguagesAxes { get; set; }
        public Func<double, string> YFormatter { get; }

        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }

        public TourRequestStats2ViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            YFormatter = value => Math.Round(value, 2).ToString();
            
            InitializeLanguageGraph();
            InitializeLocationGraph();

            BackCommand = new ExecuteMethodCommand(ShowTourRequestStats1View);
            NextCommand = new ExecuteMethodCommand(ShowTourRequestStats3View);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationBrowserView);

        }

        private void InitializeLocationGraph()
        {
            List<TourRequest> tourRequests = _tourRequestService.GetAll();
            foreach (TourRequest request in tourRequests)
            {
                request.Location = _locationService.GetById(request.Location.Id);
            }
            Locations = _tourRequestService.GetAllLocations(tourRequests);
            LocationsAxes = new string[Locations.Count];
            LocationNumberOfRequests = new List<int>();

            // Years 
            foreach (string location in Locations)
            {
                LocationNumberOfRequests.Add(_tourRequestService.GetLocationNumberOfRequests(location, tourRequests));
            }

            int i = 0;
            foreach (string location in Locations)
            {
                LocationsAxes[i] = location;
                i++;
            }
            // nadjem sve requeste za odabranu lokaciju
            // 

            LineSeries LocationNumberOfRequestsAxes = new LineSeries();
            LocationNumberOfRequestsAxes.Title = "locationRequests";
            LocationNumberOfRequestsAxes.Values = new ChartValues<int>(LocationNumberOfRequests);
            LocationNumberOfRequestsAxes.LineSmoothness = 0;
            SeriesCollectionLocation = new SeriesCollection();
            SeriesCollectionLocation.Add(LocationNumberOfRequestsAxes);
        }

        private void InitializeLanguageGraph()
        {
            GuideLanguages = new List<string>();
            LanguageNumberOfRequests = new List<int>();
            foreach (GuideLanguage l in Enum.GetValues(typeof(GuideLanguage)))
            {
                if (l == GuideLanguage.All) continue;
                GuideLanguages.Add(l.ToString());
                LanguageNumberOfRequests.Add(_tourRequestService.GetLanguageNumberOfRequests(l));
            }

            LanguagesAxes = new string[GuideLanguages.Count];
            int i = 0;
            foreach (string language in GuideLanguages)
            {
                LanguagesAxes[i] = language;
                i++;
            }

            LineSeries LanguageNumberOfRequestsAxes = new LineSeries();
            LanguageNumberOfRequestsAxes.Title = "yearRequests";
            LanguageNumberOfRequestsAxes.Values = new ChartValues<int>(LanguageNumberOfRequests);
            LanguageNumberOfRequestsAxes.LineSmoothness = 0;
            SeriesCollectionLanguage = new SeriesCollection();
            SeriesCollectionLanguage.Add(LanguageNumberOfRequestsAxes);

        }



        private void ShowTourRequestStats1View()
        {
            TourRequestStats1ViewModel tourRequestStats1ViewModel = new TourRequestStats1ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStats1ViewModel));
            navigate.Execute(null);
        }
        private void ShowTourRequestStats3View()
        {
            TourRequestStats3ViewModel tourRequestStats3ViewModel = new TourRequestStats3ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStats3ViewModel));
            navigate.Execute(null);
        }


        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowNotificationBrowserView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));
            navigate.Execute(null);
        }
    }
}
