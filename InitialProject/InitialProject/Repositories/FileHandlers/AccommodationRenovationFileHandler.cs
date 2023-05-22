using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    class AccommodationRenovationFileHandler
    {
        private const string _renovationsFilePath = "../../../Resources/Data/accommodationRenovations.csv";
        private readonly Serializer<AccommodationRenovation> _serializer;

        public AccommodationRenovationFileHandler()
        {
            _serializer = new Serializer<AccommodationRenovation>();
        }
        public List<AccommodationRenovation> Load()
        {
            var renovations = _serializer.FromCSV(_renovationsFilePath);
            FillInAccommodations(renovations);
            return renovations;
        }
        private void FillInAccommodations(List<AccommodationRenovation> renovations)
        {
            var accommodations = new AccommodationFileHandler().Load();
            renovations.ForEach(r =>
                r.Accommodation = accommodations.Find(a => a.Id == r.Accommodation.Id));
        }
        public void Save(List<AccommodationRenovation> renovations)
        {
            _serializer.ToCSV(_renovationsFilePath, renovations);
        }
    }
}

