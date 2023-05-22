using InitialProject.Application.Observer;
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
    public class GuestRatingRepository: IGuestRatingRepository
    {
        private List<GuestRating> _guestRatings;
        private readonly GuestRatingFileHandler _fileHandler;

        public GuestRatingRepository()
        {
            _fileHandler = new GuestRatingFileHandler();
            _guestRatings = _fileHandler.Load();
        }
        public List<GuestRating> GetAll()
        {
            return _fileHandler.Load();
        }

        public GuestRating Save(GuestRating guestRating)
        {
            _guestRatings = _fileHandler.Load();
            _guestRatings.Add(guestRating);
            _fileHandler.Save(_guestRatings);
            return guestRating;
        }
        public GuestRating Add(int ownerId, int guestId, int hygiene, int respectsRules, int communication, int timeliness, int noiseLevel, int overallExperience, string comment, AccommodationReservation reservation)
        {
            _guestRatings = _fileHandler.Load();
            GuestRating guestRating = new GuestRating(ownerId, guestId, hygiene, respectsRules, communication, timeliness, noiseLevel, overallExperience, comment, reservation);
            _guestRatings.Add(guestRating);
            _fileHandler.Save(_guestRatings);
            return guestRating;
        }

        public List<GuestRating> GetEligibleForDisplay(int guestId)
        {
            _guestRatings = _fileHandler.Load();
            return _guestRatings.FindAll(r => r.Reservation.Guest.Id == guestId &&
                                         r.Reservation.IsOwnerRated);
        }
    }
}
