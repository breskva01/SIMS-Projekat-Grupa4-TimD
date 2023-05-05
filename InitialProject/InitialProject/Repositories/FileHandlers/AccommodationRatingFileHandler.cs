using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class AccommodationRatingFileHandler
    {
        private const string _ratingsFilePath = "../../../Resources/Data/accommodationRatings.csv";
        private readonly Serializer<AccommodationRating> _serializer;

        public AccommodationRatingFileHandler()
        {
            _serializer = new Serializer<AccommodationRating>();
        }
        public List<AccommodationRating> Load()
        {
            var ratings = _serializer.FromCSV(_ratingsFilePath);
            FillInReservations(ratings);
            return ratings;
        }
        private void FillInReservations(List<AccommodationRating> ratings)
        {
            var reservations = new AccommodationReservationFileHandler().Load();
            ratings.ForEach(r =>
                r.Reservation = reservations.Find(res => res.Id == r.Reservation.Id));
        }
        public void Save(List<AccommodationRating> ratings)
        {
            _serializer.ToCSV(_ratingsFilePath, ratings);
        }
    }
}
