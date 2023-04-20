using InitialProject.Domain.Models;
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
        List<AccommodationReservation> GetAllNewlyCancelled(int ownerId);
        List<AccommodationReservation> GetConfirmed(int guestId);
        List<AccommodationReservation> GetEgligibleForRating(int guestId);
        List<AccommodationReservation> GetExisting(int accommodationId);
        List<AccommodationReservation> GetExistingInsideDateRange(int accommodationId, DateOnly startDate, DateOnly endDate);
        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId);
        public void updateLastNotification(AccommodationReservation accommodationReservation);
        public void updateRatingStatus(AccommodationReservation accommodationReservation);
        void Cancel(int reservationId);
        void Save(AccommodationReservation reservation);
        void MarkOwnerAsRated(int reservationId);
        public void MoveReservation(int reservationId, DateOnly newCheckIn, DateOnly newCheckout);
        public string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut);
    }
}
