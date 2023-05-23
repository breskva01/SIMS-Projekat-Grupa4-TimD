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
        public LineSeries GetYearlyReservations(int id);
        public LineSeries GetYearlyCancellations(int id);
        public LineSeries GetYearlyMovedReservations(int id);
        public LineSeries GetYearlyRenovationReccommendations(int id);
        public LineSeries GetMonthlyReservations(int id, int year);
        public LineSeries GetMonthlyCancellations(int id, int year);
        public LineSeries GetMonthlyMovedReservations(int id, int year);
        public LineSeries GetMonthlyRenovationReccommendations(int id, int year);
        public string GetMostBookedYear(int id);
        public string GetMostBookedMonth(int id, int year);
        public List<TimeSlot> GetAvailableDates(DateTime start, DateTime end, int duration, int id);
        void Update(AccommodationReservation reservation);
    }
}
