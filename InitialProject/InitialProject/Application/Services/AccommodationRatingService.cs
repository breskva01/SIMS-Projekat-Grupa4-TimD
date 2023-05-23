using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationRatingService
    {
        private readonly IAccommodationRatingRepository _ratingRepository;
        private readonly IAccommodationReservationRepository _reservationRepository;
        public AccommodationRatingService()
        {
            _ratingRepository = RepositoryInjector.Get<IAccommodationRatingRepository>();
            _reservationRepository = RepositoryInjector.Get<IAccommodationReservationRepository>();
        }
        public List<AccommodationRating> GetAll()
        {
            return _ratingRepository.GetAll();
        }
        public List<AccommodationRating> GetByOwnerId(int ownerId)
        {
            return _ratingRepository.GetByOwnerId(ownerId);
        }
        public double CalculateAccommodationAverageRating(int accommodationId)
        {
            var ratings = _ratingRepository.GetByAccommodationId(accommodationId);
            if (!ratings.Any())
                return 0.0;
            return ratings.Average(r => r.AverageRating);
        }
        public double CalculateOwnerAverageRating(int ownerId)
        {
            var ratings = _ratingRepository.GetByOwnerId(ownerId);
            if (!ratings.Any())
                return 0.0;
            return ratings.Average(r => r.AverageRating);
        }
        public List<AccommodationRating> GetEligibleForDisplay(int ownerId)
        {
            return _ratingRepository.GetEligibleForDisplay(ownerId);
        }
        public List<AccommodationReservation> GetEligibleForRating(int guestId)
        {
            var reservations = _reservationRepository.GetFilteredReservations(guestId: guestId,
                                                        status: AccommodationReservationStatus.Finished);
            return reservations.FindAll(r => r.IsEligibleForRating());
        }
        public void Save(AccommodationReservation reservation, int location, int hygiene, int pleasantness,
                         int fairness, int parking, string comment, List<string> pictureURLs,
                         bool renovatingNeeded, string renovationComment, int renovationUrgency)
        {
            if (!renovatingNeeded)
                renovationUrgency = 0;
            var rating = new AccommodationRating(reservation, location, hygiene, pleasantness, fairness, parking,
                                                 comment, pictureURLs, renovationComment, renovationUrgency);
            reservation.IsOwnerRated = true;
            _reservationRepository.Update(reservation);
            _ratingRepository.Save(rating);
        }
    }
}
