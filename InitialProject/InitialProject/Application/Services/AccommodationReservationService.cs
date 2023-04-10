using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationReservationService : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationReservationRepository _repository;
        public AccommodationReservationService()
        {
            _observers = new List<IObserver>();
            _repository = new AccommodationReservationRepository();
        }
        public bool Cancel(int reservationId)
        {
            var reservation = _repository.GetById(reservationId);
            if (!reservation.CanBeCancelled())
                return false;
            _repository.Cancel(reservationId);
            NotifyObservers();
            return true;
        }
        public List<AccommodationReservation> GetAvailable(DateOnly startDate, DateOnly endDate, int stayLength, Accommodation accommodation, User guest)
        {
            var existingReservations = _repository.GetExisting(accommodation.Id);
            var availableReservations = FindInsideDateRange(startDate, endDate, stayLength, 
                                                            accommodation, guest, existingReservations);
            if (availableReservations.Count == 0)
                availableReservations = FindOutsideDateRange(startDate, endDate, stayLength,
                                                             accommodation, guest, existingReservations);
            return availableReservations;
        }
        private List<AccommodationReservation> FindInsideDateRange(DateOnly startDate, DateOnly endDate, int stayLength, Accommodation accommodation, User guest, List<AccommodationReservation> existingReservations)
        {
            var availableReservations = new List<AccommodationReservation>();
            DateOnly checkIn = startDate;
            DateOnly checkOut = startDate.AddDays(stayLength);
            int availableReservationsCount = 0;

            while (availableReservationsCount < 3 && checkOut <= endDate)
            {
                if (IsAvailable(checkIn, checkOut, existingReservations))
                {
                    availableReservations.Add(CreateReservation(accommodation, guest, stayLength, checkIn, checkOut));
                    availableReservationsCount++;
                }
                checkIn = checkIn.AddDays(1);
                checkOut = checkOut.AddDays(1);
            }
            return availableReservations;
        }
        private List<AccommodationReservation> FindOutsideDateRange(DateOnly searchStartDate, DateOnly searchEndDate, int stayLength, Accommodation accommodation, User guest, List<AccommodationReservation> existingReservations)
        {
            var availableReservations = new List<AccommodationReservation>();
            SetStartingSearchDates(ref searchStartDate, ref searchEndDate, accommodation.Id, stayLength);
            bool isBeforeDateRange = true;
            int daysOffset = 0;
            int availableReservationsCount = 0;

            while (availableReservationsCount < 3)
            {
                DateOnly checkIn = isBeforeDateRange ? searchStartDate.AddDays(-daysOffset) : searchEndDate.AddDays(daysOffset);
                DateOnly checkOut = checkIn.AddDays(stayLength);
                if (IsAvailable(checkIn, checkOut, existingReservations))
                {
                    availableReservations.Add(CreateReservation(accommodation, guest, stayLength, checkIn, checkOut));
                    availableReservationsCount++;
                }           
                daysOffset++;
                isBeforeDateRange = !isBeforeDateRange;
            }
            return availableReservations;
        }
        private void SetStartingSearchDates(ref DateOnly startDate, ref DateOnly endDate, int accommodationId, int stayLength)
        {
            var reservationsInsideDateRange = _repository.GetExistingInsideDateRange(accommodationId, startDate, endDate);
            if (reservationsInsideDateRange.Count == 0)
                endDate = startDate.AddDays(1);
            else
            {
                startDate = reservationsInsideDateRange.Min(r => r.CheckIn).AddDays(-stayLength);
                endDate = reservationsInsideDateRange.Max(r => r.CheckOut);
            }
        }
        private bool IsAvailable(DateOnly checkIn, DateOnly checkOut, List<AccommodationReservation> existingReservations)
        {
            return !(checkIn < DateOnly.FromDateTime(DateTime.Now) ||
                    existingReservations.Any(r => r.Overlaps(checkIn, checkOut)));
        }
        private AccommodationReservation CreateReservation(Accommodation accommodation, User guest, int days, DateOnly checkIn, DateOnly checkOut)
        {
            return new AccommodationReservation(accommodation, guest, days, checkIn, checkOut, AccommodationReservationStatus.Confirmed);
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
