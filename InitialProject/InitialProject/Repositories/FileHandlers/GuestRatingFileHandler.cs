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
            return _serializer.FromCSV(_guestRatingsFilePath);
        }
        public void Save(List<GuestRating> guestRatings)
        {
            _serializer.ToCSV(_guestRatingsFilePath, guestRatings);
        }
    }
}
