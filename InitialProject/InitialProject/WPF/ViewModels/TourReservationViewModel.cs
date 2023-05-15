﻿using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourReservationViewModel : ViewModelBase
    {
        private User _user;
        public Tour SelectedTour { get; set; }
        public string AvailableSpots { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly TourReservationService _tourReservationService;

        private string _numberOfGuests;
        public  string NumberOfGuests
        {
            get
            { 
                return _numberOfGuests;
            }
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged(nameof(NumberOfGuests));
            }
        }

        private int GetNumberOfGuests()
        {
            int numberOfGuests = 0;

            try
            {
                numberOfGuests = int.Parse(NumberOfGuests);
            }
            catch
            {
                MessageBox.Show("You entered a non-number value for number of guests.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            return numberOfGuests;
        }

        public ICommand ReserveCommand { get; }
        public ICommand UseVoucherCommand { get; }
        public ICommand CancelCommand { get; }

        public TourReservationViewModel(NavigationStore navigationStore, User user, Tour tour)
        {
            _navigationStore = navigationStore;
            _tourReservationService = new TourReservationService();
            _user = user;
            SelectedTour = tour;
            AvailableSpots = (SelectedTour.MaximumGuests - SelectedTour.CurrentNumberOfGuests).ToString();

            ReserveCommand = new ExecuteMethodCommand(MakeReservation);
            UseVoucherCommand = new ReserveWithVoucherCommand(ShowVoucherView, this);
            CancelCommand = new ExecuteMethodCommand(ShowTourBrowserView);
        }

        private void MakeReservation()
        {
            int numberOfGuests = GetNumberOfGuests();
            if (numberOfGuests == 0)
            {
                MessageBox.Show("Please input a number of guests first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            if (numberOfGuests + SelectedTour.CurrentNumberOfGuests > SelectedTour.MaximumGuests)
            {
                MessageBox.Show("Unfortunately, there is not enough available spots for that many guests." +
                    " Try lowering the guest number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            TourReservation tourReservation = _tourReservationService.CreateReservation(SelectedTour.Id, _user.Id, numberOfGuests, false);

            ShowTourBrowserView();
        }

        private void ShowVoucherView()
        {
            VoucherBrowserViewModel voucherBrowserViewModel = new VoucherBrowserViewModel(_navigationStore, _user, SelectedTour, GetNumberOfGuests());
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, voucherBrowserViewModel));
            navigate.Execute(null);
        }

        private void ShowTourBrowserView()
        {
            NewTourBrowserViewModel newTourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, newTourBrowserViewModel));
            navigate.Execute(null);
        }
        
    }
}
