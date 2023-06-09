using InitialProject.Domain.Models;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository : IRepository<AccommodationReservation>
    {
        AccommodationReservation GetById(int reservationId);
        List<AccommodationReservation> GetFilteredReservations(int? guestId = null, int? accommodationId = null, DateOnly? startDate = null,
            DateOnly? endDate = null, AccommodationReservationStatus status = AccommodationReservationStatus.Active);
        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId);
        void Save(AccommodationReservation reservation);
        public string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut);
        public List<TimeSlot> GetAvailableDates(DateTime start, DateTime end, int duration, int id);
        void Update(AccommodationReservation reservation);
    }
}
