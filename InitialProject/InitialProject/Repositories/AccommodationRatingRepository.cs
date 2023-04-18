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
        private List<AccommodationRating> _ratings;
        private readonly IAccommodationReservationRepository _reservationRepository;

        public AccommodationRatingRepository()
        {
            _fileHandler = new AccommodationRatingFileHandler();
            _ratings = _fileHandler.Load();
            _reservationRepository = RepositoryStore.GetIAccommodationReservationRepository;
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
        }
    }
}
