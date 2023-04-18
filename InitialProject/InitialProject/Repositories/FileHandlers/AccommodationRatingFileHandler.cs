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
            return _serializer.FromCSV(_ratingsFilePath);
        }
        public void Save(List<AccommodationRating> ratings)
        {
            _serializer.ToCSV(_ratingsFilePath, ratings);
        }
    }
}
