﻿using InitialProject.Controller;
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

namespace InitialProject.WPF.ViewModels
{
    public class AccommodationReservationViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _service;
        private readonly NavigationStore _navigationStore;
        public Accommodation Accommodation { get; set; }
        public User Guest { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICommand FindAvailableReservationsCommand { get; }
        public ICommand ShowAccommodationBrowserViewCommand { get; }
        public AccommodationReservationViewModel(NavigationStore navigationStore ,User user, Accommodation accommodation)
        {
            _navigationStore = navigationStore;
            Guest = user;
            Accommodation = accommodation;
            _service = new AccommodationReservationService();
            FindAvailableReservationsCommand = new ExecuteMethodCommand(GetAvailableReservations);
            ShowAccommodationBrowserViewCommand = new ExecuteMethodCommand(ShowAccommodationBrowserView);
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
                List<AccommodationReservation> reservations = _service.GetAvailable(startDate, endDate, Days, Accommodation, Guest);
                ShowDatePickerView(reservations);
                
            }
            else
                MessageBox.Show("Izaberite željeni opseg datuma");
        }
        private void ShowDatePickerView(List<AccommodationReservation> reservations)
        {
            var viewModel = new AccommodationReservationDatePickerViewModel(_navigationStore, reservations);
            var navigateCommand = new NavigateCommand
                (new NavigationService(_navigationStore, viewModel));

            navigateCommand.Execute(null);
        }
        private void ShowAccommodationBrowserView()
        {
            var viewModel = new AccommodationBrowserViewModel(_navigationStore, Guest);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, viewModel));
            navigateCommand.Execute(null);
        }
    }
}
