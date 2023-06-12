﻿using InitialProject.Application.Injector;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using InitialProject.Repository;
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
        private readonly IAccommodationRepository _accommodationRepository;
        private List<AccommodationRating> _ratings;
        private List<User> _users;

        public AccommodationRatingRepository()
        {
            _fileHandler = new AccommodationRatingFileHandler();
            _userFileHandler = new UserFileHandler();
            _accommodationRepository = RepositoryInjector.Get<IAccommodationRepository>();
        }

        public List<AccommodationRating> GetAll()
        {
            return _ratings = _fileHandler.Load();
        }
        public List<AccommodationRating> GetByOwnerId(int ownerId)
        {
            GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId);
        }
        public List<AccommodationRating> GetByAccommodationId(int accommodationId)
        {
            GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.Id == accommodationId);
        }
        public List<AccommodationRating> GetEligibleForDisplay(int ownerId)
        {
            GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId &&
                                         r.Reservation.IsGuestRated);
        }
        public void Save(AccommodationRating rating)
        {
            GetAll();
            _ratings.Add(rating);
            _fileHandler.Save(_ratings);
            var accommodations = _accommodationRepository.GetAll();
            _ratings.ForEach(r => r.Reservation.Accommodation = accommodations.Find(acc => acc.Id == r.Reservation.Accommodation.Id));
            double[] totalAverageRating = CalculateTotalAverageOwnerRating(rating);
            UpdateSuperOwnerStatus(rating.Reservation.Accommodation.Owner.Id, totalAverageRating);
        }
        private double[] CalculateTotalAverageOwnerRating(AccommodationRating rating)
        {
            int[] ratings = { rating.Location, rating.Hygiene, rating.Pleasantness, rating.Fairness, rating.Parking };
            double averageRating = ratings.Average();
            int OwnerRatingsCount = 1;
            double[] totalAverageRating = new double[2];

            foreach (AccommodationRating ar in _ratings)
            {
                double[] previousAverageRatings = { ar.Location, ar.Hygiene, ar.Pleasantness, ar.Fairness, ar.Parking };
                averageRating += previousAverageRatings.Average();
                OwnerRatingsCount++;
            }

            totalAverageRating[0] = averageRating / OwnerRatingsCount;
            totalAverageRating[1] = OwnerRatingsCount;
            return totalAverageRating;
        }
        public void UpdateSuperOwnerStatus(int ownerId, double[] totalAverageRating)
        {
            Owner newOwner = new Owner();
            _users = _userFileHandler.Load();
            _ratings = GetByOwnerId(ownerId);
            Owner owner = (Owner)_users.Find(o => o.Id == ownerId);
            double OwnerRatingsCount = totalAverageRating[1];
            owner.SuperOwner = (totalAverageRating[0] >= 4.5 && OwnerRatingsCount >= 50) ? true : false;
            owner.Rating = Math.Round(totalAverageRating[0],2); 
            newOwner = owner;
            _users.Remove(owner);
            _users.Add(newOwner);
            _userFileHandler.Save(_users);
        }
    }
}
