using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        List<AccommodationReservation> GetAll();
        AccommodationReservation GetById(int reservationId);
        List<AccommodationReservation> GetConfirmed(int guestId);
        List<AccommodationReservation> GetEgligibleForRating(int guestId);
        List<AccommodationReservation> GetExisting(int accommodationId);
        List<AccommodationReservation> GetExistingInsideDateRange(int accommodationId, DateOnly startDate, DateOnly endDate);
        void Cancel(int reservationId);
        void Save(AccommodationReservation reservation);
        void MarkOwnerAsRated(int reservationId);
    }
}
