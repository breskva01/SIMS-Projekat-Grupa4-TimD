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
        private const string _accommodationsFilePath = "../../../Resources/Data/accommodations.csv";
        private const string _guestsFilePath = "../../../Resources/Data/users.csv";
        private readonly Serializer<AccommodationReservation> _serializer;

        public AccommodationReservationFileHandler()
        {
            _serializer = new Serializer<AccommodationReservation>();
        }

        public List<AccommodationReservation> Load()
        {
            var reservations = _serializer.FromCSV(_reservationsFilePath);
            FillInAccommodations(reservations);
            FillInGuests(reservations);
            return reservations;
        }
        private void FillInAccommodations(List<AccommodationReservation> reservations)
        {
            AccommodationFileHandler fileHandler = new AccommodationFileHandler();
            List<Accommodation> accommodations = fileHandler.Load();
            foreach (var reservation in reservations)
            {
                reservation.Accommodation = accommodations.FirstOrDefault(a => a.Id == reservation.AccommodationId);
            }
        }
        private void FillInGuests(List<AccommodationReservation> reservations)
        {
            Storage<User> storage = new Storage<User>(_guestsFilePath);
            List<User> guests = storage.Load();
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
