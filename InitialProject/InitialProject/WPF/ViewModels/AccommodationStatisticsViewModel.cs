using InitialProject.Domain.Models;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using System.Windows.Controls;
using InitialProject.Application.Services;
using System.Collections.ObjectModel;
using InitialProject.WPF.NewViews;
using System.Windows.Input;
using InitialProject.Application.Commands;

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationStatisticsViewModel : ViewModelBase
    {
        private Accommodation _selectedAccommodation;
        private AccommodationStatisticsService _accommodationStatisticsService;
        public PlotModel Reservations { get; set; }
        public PlotModel Cancellations { get; set; }
        public PlotModel MovedReservations { get; set; }
        public PlotModel RenovationReccommendations { get; set; }
        public ObservableCollection<Location> PopularLocations { get; set; }
        public ObservableCollection<Location> UnpopularLocations { get; set; }
        public Location SelectedPopularLocation { get; set; }
        public Location SelectedUnpopularLocation { get; set; }
        private User _owner;
        private string _mostBooked;
        public string MostBooked
        {
            get => _mostBooked;
            set
            {
                if (value != _mostBooked)
                {
                    _mostBooked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _yearPicked;

        private string _year;
        public string Year
        {
            get => _year;
            set
            {
                if (value != _year)
                {
                    _year = value;
                    _yearPicked = true;
                    OnPropertyChanged();
                    if (PerMonthSelected)
                    {
                        ShowMonthlyGraphs();
                        DisplayMostBookedMonth();
                    }
                }
            }
        }
        private bool _perYearSelected;
        public bool PerYearSelected
        {
            get => _perYearSelected;
            set
            {
                if (value != _perYearSelected)
                {
                    _perYearSelected = value;
                    OnPropertyChanged();
                    if (PerYearSelected)
                    {
                        ShowYearlyGraphs();
                        DisplayMostBookedYear();
                    }
                }
            }
        }
        private bool _perMonthSelected;
        public bool PerMonthSelected
        {
            get => _perMonthSelected;
            set
            {
                if (value != _perMonthSelected)
                {
                    _perMonthSelected = value;
                    OnPropertyChanged();
                    if (PerMonthSelected && _yearPicked)
                    {
                        ShowMonthlyGraphs();
                        DisplayMostBookedMonth();
                    }
                }
            }
        }
        public ICommand NewAccommodationCommand { get; }
        public AccommodationStatisticsViewModel(Accommodation selectedAccommodation, User owner)
        {
            _owner = owner;
            _selectedAccommodation = selectedAccommodation;
            Reservations = new PlotModel { Title = "Reservations" };
            Cancellations = new PlotModel { Title = "Cancellations" };
            MovedReservations = new PlotModel { Title = "Moved reservations" };
            RenovationReccommendations = new PlotModel { Title = "Reccommendations" };
            _accommodationStatisticsService = new AccommodationStatisticsService();
            PopularLocations = new ObservableCollection<Location>(_accommodationStatisticsService.GetMostPopularLocations());
            UnpopularLocations = new ObservableCollection<Location>(_accommodationStatisticsService.GetMostUnpopularLocations());
            NewAccommodationCommand = new ExecuteMethodCommand(NewAccommodation);
            ShowYearlyGraphs();

        }
        private void ShowYearlyGraphs()
        {
            Reservations.Series.Clear();
            Cancellations.Series.Clear();
            MovedReservations.Series.Clear();
            RenovationReccommendations.Series.Clear();
            var series = _accommodationStatisticsService.GetYearlyReservations(_selectedAccommodation.Id);
            Reservations.Series.Add(series);
            series = _accommodationStatisticsService.GetYearlyCancellations(_selectedAccommodation.Id);
            Cancellations.Series.Add(series);
            series = _accommodationStatisticsService.GetYearlyMovedReservations(_selectedAccommodation.Id);
            MovedReservations.Series.Add(series);
            series = _accommodationStatisticsService.GetYearlyRenovationReccommendations(_selectedAccommodation.Id);
            RenovationReccommendations.Series.Add(series);
            Reservations.Axes.Clear();
            Cancellations.Axes.Clear();
            MovedReservations.Axes.Clear();
            RenovationReccommendations.Axes.Clear();
            Reservations.InvalidatePlot(true);
            Cancellations.InvalidatePlot(true);
            MovedReservations.InvalidatePlot(true);
            RenovationReccommendations.InvalidatePlot(true);
        }
        private void ShowMonthlyGraphs()
        {
            Reservations.Series.Clear();
            Cancellations.Series.Clear();
            MovedReservations.Series.Clear();
            RenovationReccommendations.Series.Clear();
            
            var series = _accommodationStatisticsService.GetMonthlyReservations(_selectedAccommodation.Id, int.Parse(Year));
            Reservations.Series.Add(series);
            series = _accommodationStatisticsService.GetMonthlyCancellations(_selectedAccommodation.Id, int.Parse(Year));
            Cancellations.Series.Add(series);
            series = _accommodationStatisticsService.GetMonthlyMovedReservations(_selectedAccommodation.Id, int.Parse(Year));
            MovedReservations.Series.Add(series);
            series = _accommodationStatisticsService.GetMonthlyRenovationReccommendations(_selectedAccommodation.Id, int.Parse(Year));
            RenovationReccommendations.Series.Add(series);
            addXAxisLabels();
            Reservations.InvalidatePlot(true);
            Cancellations.InvalidatePlot(true);
            MovedReservations.InvalidatePlot(true);
            RenovationReccommendations.InvalidatePlot(true);
        }
        private void addXAxisLabels()
        {
            var xReservations = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Month",
                Angle = 45
            };
            var xCancellations = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Month",
                Angle = 45
            };
            var xMovedReservations = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Month",
                Angle = 45
            };
            var xRenovationReccommendations = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Month",
                Angle = 45
            };
            List<string> months = new List<string>
                {
                    "January", "February", "March", "April", "May", "June",
                    "July", "August", "September", "October", "November", "December"
                };
            xReservations.Labels.AddRange(months);
            xCancellations.Labels.AddRange(months);
            xMovedReservations.Labels.AddRange(months);
            xRenovationReccommendations.Labels.AddRange(months);

            Reservations.Axes.Clear();
            Cancellations.Axes.Clear();
            MovedReservations.Axes.Clear();
            RenovationReccommendations.Axes.Clear();

            Reservations.Axes.Add(xReservations);
            Cancellations.Axes.Add(xCancellations);
            MovedReservations.Axes.Add(xMovedReservations);
            RenovationReccommendations.Axes.Add(xRenovationReccommendations);
        }
        private void DisplayMostBookedYear()
        {
            MostBooked = _accommodationStatisticsService.GetMostBookedYear(_selectedAccommodation.Id);
        }
        private void DisplayMostBookedMonth()
        {
            MostBooked = _accommodationStatisticsService.GetMostBookedMonth(_selectedAccommodation.Id, int.Parse(Year));
        }
        private void NewAccommodation() 
        {
            AccommodationRegistrationView accommodationRegistrationView = new AccommodationRegistrationView(_owner);
            accommodationRegistrationView.Show();
        }
    }
}
