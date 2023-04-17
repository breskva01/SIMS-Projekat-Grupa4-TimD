using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class TourReservationFileHandler
    {
        private const string _tourReservationsFilePath = "../../../Resources/Data/tourReservations.csv";
        private readonly Serializer<TourReservation> _serializer;

        public TourReservationFileHandler()
        {
            _serializer = new Serializer<TourReservation>();
        }
        public List<TourReservation> Load()
        {
            return _serializer.FromCSV(_tourReservationsFilePath);
        }

        public void Save(List<TourReservation> list)
        {
            _serializer.ToCSV(_tourReservationsFilePath, list);
        }
    }
}
