﻿using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class GuestRatingViewModel: ViewModelBase
    {
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly NavigationStore _navigationStore;
        private readonly AccommodationService _accommodationService;
        private readonly UserService _userService;
        private readonly GuestRatingService _guestRatingService;
        private readonly User _user;
        public readonly Accommodation Accommodation;
        private List<Accommodation> _accommodations;
        private List<User> _users;
        public bool IsNotified { get; set; }
        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        private string _hygiene;
        public string Hygiene
        {
            get => _hygiene;
            set
            {
                if (value != _hygiene)
                {
                    _hygiene = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _respectsRules;
        public string RespectsRules
        {
            get => _respectsRules;
            set
            {
                if (value != _respectsRules)
                {
                    _respectsRules = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _communication;
        public string Communication
        {
            get => _communication;
            set
            {
                if (value != _communication)
                {
                    _communication = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _timeliness;
        public string Timeliness
        {
            get => _timeliness;
            set
            {
                if (value != _timeliness)
                {
                    _timeliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _noiseLevel;
        public string NoiseLevel
        {
            get => _noiseLevel;
            set
            {
                if (value != _noiseLevel)
                {
                    _noiseLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _overallExperience;
        public string OverallExperience
        {
            get => _overallExperience;
            set
            {
                if (value != _overallExperience)
                {
                    _overallExperience = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuestRatingViewModel(NavigationStore navigationStore, User user, bool isNotified)
        {
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationService = new AccommodationService();
            _guestRatingService = new GuestRatingService();
            _navigationStore = navigationStore;
            _accommodations = _accommodationService.GetAll();
            _user = user;
            IsNotified = true;
            AccommodationReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.FindCompletedAndUnrated(_user.Id));
            InitializeCommands();
        }
        
        public ICommand RateGuestCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackNavigateCommand =>
            new NavigateCommand(new NavigationService(_navigationStore, GoBack()));
        private void InitializeCommands()
        {
            RateGuestCommand = new ExecuteMethodCommand(RateGuest);
            BackCommand = new ExecuteMethodCommand(Back);
        }

        private void RateGuest()
        {
            int ownerId = SelectedReservation.Accommodation.Owner.Id;
            int guestId = SelectedReservation.Guest.Id;
            GuestRating guestRating = _guestRatingService.RateGuest(ownerId, guestId, int.Parse(Hygiene), int.Parse(RespectsRules), int.Parse(Communication), int.Parse(Timeliness), int.Parse(NoiseLevel), int.Parse(OverallExperience), Comment);
            _accommodationReservationService.updateRatingStatus(SelectedReservation);
            AccommodationReservations.Remove(SelectedReservation);
            ResetData();
        }
        private void Back()
        {
            BackNavigateCommand.Execute(null);
        }
        private void ResetData()
        {
            Hygiene = null;
            RespectsRules = null;
            Communication = null;
            Timeliness = null;
            NoiseLevel = null;
            OverallExperience = null;
            Comment = "";
        }
        private OwnerProfileViewModel GoBack()
        {
            return new OwnerProfileViewModel(_navigationStore, (Owner)_user, IsNotified);
        }


    }
}
