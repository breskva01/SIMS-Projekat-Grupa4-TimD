using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class GuestRatingFileHandler
    {
        private const string _guestRatingsFilePath = "../../../Resources/Data/guestRatings.csv";
        private readonly Serializer<GuestRating> _serializer;

        public GuestRatingFileHandler()
        {
            _serializer = new Serializer<GuestRating>();
        }
        public List<GuestRating> Load()
        {
            var ratings = _serializer.FromCSV(_guestRatingsFilePath);
            FillInReservations(ratings);
            return ratings;
        }
        public void Save(List<GuestRating> guestRatings)
        {
            _serializer.ToCSV(_guestRatingsFilePath, guestRatings);
        }
        private void FillInReservations(List<GuestRating> ratings)
        {
            var reservations = new AccommodationReservationFileHandler().Load();
            ratings.ForEach(r =>
                r.Reservation = reservations.Find(res => res.Id == r.Reservation.Id));
        }
    }
}
