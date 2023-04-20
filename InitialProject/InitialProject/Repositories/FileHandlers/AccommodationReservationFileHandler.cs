using InitialProject.Application.Injector;
using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
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
            FillInAccommodations(reservations);
            return reservations;
        }
        private void FillInGuests(List<AccommodationReservation> reservations)
        {
            var users = new UserFileHandler().Load();
            reservations.ForEach(r =>
                r.Guest = users.Find(u => u.Id == r.Guest.Id));
        }
        private void FillInAccommodations(List<AccommodationReservation> reservations)
        {
            var accommodations = new AccommodationFileHandler().Load();
            reservations.ForEach(r =>
                r.Accommodation = accommodations.Find(a => a.Id == r.Accommodation.Id));
        }
        public void Save(List<AccommodationReservation> reservations)
        {
            _serializer.ToCSV(_reservationsFilePath, reservations);
        }
    }
}
