using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class TourFileHandler
    {
        private const string _toursFilePath = "../../../Resources/Data/tours.csv";
        private readonly Serializer<Tour> _serializer;

        public TourFileHandler()
        {
            _serializer = new Serializer<Tour>();
        }
        public List<Tour> Load()
        {
            return _serializer.FromCSV(_toursFilePath);
        }
        public void Save(List<Tour> tours)
        {
            _serializer.ToCSV(_toursFilePath, tours);
        }
    }
}
