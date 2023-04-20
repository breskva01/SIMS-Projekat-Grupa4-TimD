using InitialProject.Application.Injector;
using InitialProject.Application.Stores;
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
            _reservationRepository = RepositoryInjector.Get<IAccommodationReservationRepository>();
            _userRepository = RepositoryInjector.Get<IUserRepository>();
        }

        public List<AccommodationRating> GetAll()
        {
            _ratings = _fileHandler.Load();
            var reservations = _reservationRepository.GetAll();
            _ratings.ForEach(r => r.Reservation = reservations.Find(res => res.Id == r.Reservation.Id));
            return _ratings;
        }
        public List<AccommodationRating> GetByOwnerId(int ownerId)
        {
            _ratings = GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId);
        }
        public List<AccommodationRating> GetEgligibleForDisplay(int ownerId)
        {
            _ratings = GetAll();
            return _ratings.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId &&
                                         r.Reservation.IsGuestRated);
        }
        public void Save(AccommodationRating rating)
        {
            _ratings = _fileHandler.Load();
            _ratings.Add(rating);
            _fileHandler.Save(_ratings);
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
            User newOwner = new User();
            _users = _userFileHandler.Load();
            _ratings = GetByOwnerId(ownerId);
            User owner = _users.Find(o => o.Id == ownerId);
            double OwnerRatingsCount = totalAverageRating[1];
            owner.SuperOwner = (totalAverageRating[0] >= 4.5 && OwnerRatingsCount >= 2) ? true : false;  

            newOwner = owner;
            _users.Remove(owner);
            _users.Add(newOwner);
            _userFileHandler.Save(_users);
        }
    }
}
