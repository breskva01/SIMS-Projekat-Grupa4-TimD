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

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationStatisticsViewModel : ViewModelBase
    {
        private Accommodation _selectedAccommodation;
        private AccommodationReservationService _accommodationReservationService;
        public PlotModel Reservations { get; set; }
        public PlotModel Cancellations { get; set; }
        public PlotModel MovedReservations { get; set; }
        private string _mostBooked;
        public string MostBooked 
        {
            get => _mostBooked;
            set 
            {
                if(value != _mostBooked)
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
                    _yearPicked= true;
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
        public AccommodationStatisticsViewModel(Accommodation selectedAccommodation)
        {
            _selectedAccommodation = selectedAccommodation;
            Reservations = new PlotModel { Title = "Reservations" };
            Cancellations = new PlotModel { Title = "Cancellations" };
            MovedReservations = new PlotModel { Title = "Moved reservations" };
            _accommodationReservationService = new AccommodationReservationService();
            ShowYearlyGraphs();

        }
        private void ShowYearlyGraphs()
        {
            Reservations.Series.Clear();
            Cancellations.Series.Clear();
            MovedReservations.Series.Clear();
            var series = _accommodationReservationService.GetYearlyReservations(_selectedAccommodation.Id);
            Reservations.Series.Add(series);
            series = _accommodationReservationService.GetYearlyCancellations(_selectedAccommodation.Id);
            Cancellations.Series.Add(series);
            series = _accommodationReservationService.GetYearlyMovedReservations(_selectedAccommodation.Id);
            MovedReservations.Series.Add(series);
            Reservations.Axes.Clear();
            Cancellations.Axes.Clear();
            MovedReservations.Axes.Clear();
            Reservations.InvalidatePlot(true);
            Cancellations.InvalidatePlot(true);
            MovedReservations.InvalidatePlot(true);
        }
        private void ShowMonthlyGraphs()
        {
            Reservations.Series.Clear();
            Cancellations.Series.Clear();
            MovedReservations.Series.Clear();
            if (Year == null)
            {
                var series = _accommodationReservationService.GetMonthlyReservations(_selectedAccommodation.Id, DateTime.Now.Year);
                Reservations.Series.Add(series);
                series = _accommodationReservationService.GetMonthlyCancellations(_selectedAccommodation.Id, DateTime.Now.Year);
                Cancellations.Series.Add(series);
                series = _accommodationReservationService.GetMonthlyMovedReservations(_selectedAccommodation.Id, DateTime.Now.Year);
                MovedReservations.Series.Add(series);
                addXAxisLabels();
                Reservations.InvalidatePlot(true);
                Cancellations.InvalidatePlot(true);
                MovedReservations.InvalidatePlot(true);
            }
            else
            {
                var series = _accommodationReservationService.GetMonthlyReservations(_selectedAccommodation.Id, int.Parse(Year));
                Reservations.Series.Add(series);
                series = _accommodationReservationService.GetMonthlyCancellations(_selectedAccommodation.Id, int.Parse(Year));
                Cancellations.Series.Add(series);
                series = _accommodationReservationService.GetMonthlyMovedReservations(_selectedAccommodation.Id, int.Parse(Year));
                MovedReservations.Series.Add(series);
                addXAxisLabels();
                Reservations.InvalidatePlot(true);
                Cancellations.InvalidatePlot(true);
                MovedReservations.InvalidatePlot(true);
            }
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
                List<string> months = new List<string>
                {
                    "January", "February", "March", "April", "May", "June",
                    "July", "August", "September", "October", "November", "December"
                };
                xReservations.Labels.AddRange(months);
                xCancellations.Labels.AddRange(months);
                xMovedReservations.Labels.AddRange(months);

                Reservations.Axes.Clear();
                Cancellations.Axes.Clear();
                MovedReservations.Axes.Clear();

                Reservations.Axes.Add(xReservations);
                Cancellations.Axes.Add(xCancellations);
                MovedReservations.Axes.Add(xMovedReservations);
        }
        private void DisplayMostBookedYear()
        {
            MostBooked = _accommodationReservationService.GetMostBookedYear(_selectedAccommodation.Id);
        }
        private void DisplayMostBookedMonth()
        {
            MostBooked = _accommodationReservationService.GetMostBookedMonth(_selectedAccommodation.Id, int.Parse(Year));
        }
    }
}
