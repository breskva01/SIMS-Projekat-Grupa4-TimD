﻿using InitialProject.Application.Services;
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
    public class TourGuestsViewModel : ViewModelBase
    {
        private TourReservationService _tourReservationService;
        private UserService _userService;

        private readonly ObservableCollection<TourReservation> _tourReservations;
        private readonly ObservableCollection<User> _users;
        private readonly ObservableCollection<User> _guests;

        public IEnumerable<User> Guests => _guests;




        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourGuestsViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourReservationService = new TourReservationService();
            _userService = new UserService();
            _guests = new ObservableCollection<User>();

            _tourReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            _users = new ObservableCollection<User>(_userService.GetAll());

            foreach (TourReservation res in _tourReservations)
            {
                foreach (User u in _users)
                {
                    if (res.GuestId == u.Id && !_guests.Contains(u))
                    {
                        _guests.Add(u);
                    }

                }
            }
        }
    }
}
