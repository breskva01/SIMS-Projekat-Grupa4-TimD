using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class AccommodationReservationFileHandler
    {
        private const string _reservationsFilePath = "../../../Resources/Data/accommodationReservations.csv";
        private readonly Serializer<AccommodationReservation> _serializer;

        public AccommodationReservationFileHandler()
        {
            _serializer = new Serializer<AccommodationReservation>();
        }

        public List<AccommodationReservation> Load()
        {
            return _serializer.FromCSV(_reservationsFilePath);
        }
        public void Save(List<AccommodationReservation> reservations)
        {
            _serializer.ToCSV(_reservationsFilePath, reservations);
        }
    }
}
