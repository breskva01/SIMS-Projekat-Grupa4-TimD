﻿using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationRatingRepository : IAccommodationRatingRepository
    {
        private readonly AccommodationRatingFileHandler _fileHandler;
        private readonly UserFileHandler _userFileHandler;
        private List<AccommodationRating> _ratings;
        private List<User> _users;
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;

        public AccommodationRatingRepository()
        {
            _fileHandler = new AccommodationRatingFileHandler();
            _userFileHandler = new UserFileHandler();
            _ratings = _fileHandler.Load();
            _reservationRepository = RepositoryStore.GetIAccommodationReservationRepository;
            _userRepository = RepositoryStore.GetIUserRepository;
        }

        public List<AccommodationRating> GetAll()
        {
            _ratings = _fileHandler.Load();
            var reservations = _reservationRepository.GetAll();
            _ratings.ForEach(r => r.Reservation = reservations.Find(res => res.Id == r.ReservationId));
            return _ratings;
        }
        public List<AccommodationRating> GetByOwnerId(int ownerId)
        {
            _ratings = GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.OwnerId == ownerId);
        }
        public List<AccommodationRating> GetEgligibleForDisplay(int ownerId)
        {
            _ratings = GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.OwnerId == ownerId &&
                                         r.Reservation.IsGuestRated);
        }
        public void Save(AccommodationRating rating)
        {
            _ratings = _fileHandler.Load();
            _ratings.Add(rating);
            _fileHandler.Save(_ratings);
            double averageRating = CalculateAverageOwnerRating(rating);
            UpdateSuperOwnerStatus(rating.Reservation.Accommodation.OwnerId, averageRating);
        }
        public double CalculateAverageOwnerRating(AccommodationRating rating)
        {
            int[] ratings = { rating.Location, rating.Hygiene, rating.Pleasantness, rating.Fairness, rating.Parking };
            double averageRating = ratings.Average();
            return averageRating;
        }
        public void UpdateSuperOwnerStatus(int ownerId, double averageRating)
        {
            User newOwner = new User();
            _users = _userFileHandler.Load();
            _ratings = GetByOwnerId(ownerId);
            User owner = _users.Find(o => o.Id == ownerId);
            int OwnerRatingsCount = 1;
            double totalAverageRating;
            AccommodationReservation accommodationReservation = new AccommodationReservation();

            foreach (AccommodationRating ar in _ratings)
            {
                double[] rating = { ar.Location, ar.Hygiene, ar.Pleasantness, ar.Fairness, ar.Parking };
                averageRating += rating.Average();
                OwnerRatingsCount++;
            }

            totalAverageRating = averageRating / OwnerRatingsCount;
            owner.SuperOwner = (totalAverageRating >= 4.5 && OwnerRatingsCount >= 2) ? true : false;  

            newOwner = owner;
            _users.Remove(owner);
            _users.Add(newOwner);
            _userFileHandler.Save(_users);
        }
    }
}
