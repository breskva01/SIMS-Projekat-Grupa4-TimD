using InitialProject.Application.Serializer;
using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandler
{
    public class AccommodationFileHandler
    {
        private const string _accommodationsFilePath = "../../../Resources/Data/accommodations.csv";
        private readonly Serializer<Accommodation> _serializer;

        public AccommodationFileHandler()
        {
            _serializer = new Serializer<Accommodation>();
        }
        public List<Accommodation> Load()
        {
            return _serializer.FromCSV(_accommodationsFilePath);
        }
        public void Save(List<Accommodation> accommmodations)
        {
            _serializer.ToCSV(_accommodationsFilePath, accommmodations);
        }
    }
}
