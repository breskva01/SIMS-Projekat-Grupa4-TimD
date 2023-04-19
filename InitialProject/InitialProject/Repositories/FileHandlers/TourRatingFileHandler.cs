using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class TourRatingFileHandler
    {
        private const string _tourRatingsFilePath = "../../../Resources/Data/tourRatings.csv";
        private readonly Serializer<TourRating> _serializer;

        public TourRatingFileHandler()
        {
            _serializer = new Serializer<TourRating>();
        }
        public List<TourRating> Load()
        {
            return _serializer.FromCSV(_tourRatingsFilePath);
        }
        public void Save(List<TourRating> list)
        {
            _serializer.ToCSV(_tourRatingsFilePath, list);
        }
    }
}
