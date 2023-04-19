using InitialProject.Application.Serializer;
using InitialProject.Application.Storage;
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
            var reservations = _serializer.FromCSV(_reservationsFilePath);
            FillInGuests(reservations);
            return reservations;
        }
        private void FillInGuests(List<AccommodationReservation> reservations)
        {
            var fileHandler = new UserFileHandler();
            List<User> guests = fileHandler.Load();
            foreach (var reservation in reservations)
            {
                reservation.Guest = guests.FirstOrDefault(g => g.Id == reservation.GuestId);
            }
        }
        public void Save(List<AccommodationReservation> reservations)
        {
            _serializer.ToCSV(_reservationsFilePath, reservations);
        }
    }
}
