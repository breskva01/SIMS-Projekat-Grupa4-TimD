using InitialProject.Application.Stores;
using InitialProject.Application.Stores;
using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Application.Injector;

namespace InitialProject.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;
        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();
        }
        public List<AccommodationReservation> GetAll()
        {
            return _reservations = _fileHandler.Load();
        }
        public AccommodationReservation GetById(int reservationId)
        {
            GetAll();
            return _reservations.Find(r => r.Id == reservationId);
        }
        public List<AccommodationReservation> GetExistingGuestReservations(int guestId)
        {
            GetAll();
            var activeReservations = _reservations.FindAll(r => 
                                             r.Guest.Id == guestId &&
                                             r.Status == AccommodationReservationStatus.Active);
            return activeReservations.OrderBy(r => r.CheckIn).ToList();
        }
        public List<AccommodationReservation> GetExistingReservationsForAccommodation(int accommodationId)
        {
            GetAll();
            return _reservations.FindAll(r => r.Accommodation.Id == accommodationId &&
                                              r.Status == AccommodationReservationStatus.Active);
        }
        public List<AccommodationReservation> GetExistingInsideDateRange(int accommodationId, DateOnly startDate, DateOnly endDate)
        {
            GetAll();    
            return _reservations.FindAll(r => r.Accommodation.Id == accommodationId &&
                                              r.Status == AccommodationReservationStatus.Active &&
                                              r.CheckIn >= startDate && r.CheckOut <= endDate);
        }
        public void Cancel(int reservationId)
        {
            var reservation = GetById(reservationId);
            reservation.Status = AccommodationReservationStatus.Cancelled;
            _fileHandler.Save(_reservations);
        }
        private bool IsCompleted(AccommodationReservation accommodationReservation, int ownerId)
        {
            return accommodationReservation.CheckOut < DateOnly.FromDateTime(DateTime.Now)
                    && DateOnly.FromDateTime(DateTime.Now) < accommodationReservation.CheckOut.AddDays(5)
                    && accommodationReservation.Accommodation.Owner.Id == ownerId;
        }

        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId)
        {
            GetAll();
            List<AccommodationReservation> completedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _reservations)
            {
                if (IsCompleted(reservation, ownerId) && !reservation.IsGuestRated)
                {
                    completedReservations.Add(reservation);
                }
            }
            return completedReservations;
        }
        public void updateLastNotification(AccommodationReservation accommodationReservation)
        {
            GetAll();
            AccommodationReservation newAccommodationReservation = new AccommodationReservation();
            newAccommodationReservation = accommodationReservation;
            newAccommodationReservation.LastNotification = newAccommodationReservation.LastNotification.AddDays(1);
            _reservations.Remove(accommodationReservation);
            _reservations.Add(newAccommodationReservation);
            _fileHandler.Save(_reservations);
        }
        public void updateRatingStatus(AccommodationReservation accommodationReservation)
        {
            GetAll();
            AccommodationReservation newAccommodationReservation = _reservations.Find(a => a.Id == accommodationReservation.Id);
            newAccommodationReservation.IsGuestRated = true;
            _fileHandler.Save(_reservations);
        }
        public void Save(AccommodationReservation reservation)
        {
            GetAll();
            reservation.Id = NextId();
            _reservations.Add(reservation);
            _fileHandler.Save(_reservations);
        }
        private int NextId()
        {
            GetAll();
            return _reservations?.Max(r => r.Id) + 1 ?? 0;
        }

        public List<AccommodationReservation> GetEligibleForRating(int guestId)
        {
            GetAll();
            DateOnly cutoffDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5));
            var eligibleReservations = _reservations.FindAll(r => 
                                         r.Guest.Id == guestId && r.IsOwnerRated == false &&
                                         r.Status == AccommodationReservationStatus.Finished &&
                                         r.CheckOut >= cutoffDate);
            return eligibleReservations.OrderBy(r => r.CheckIn).ToList();
        }

        public void MarkOwnerAsRated(int reservationId)
        {
            var reservation = GetById(reservationId);
            reservation.IsOwnerRated = true;
            _fileHandler.Save(_reservations);
        }

        public List<AccommodationReservation> GetAllNewlyCancelled(int ownerId)
        {
            GetAll();
            var notifactions = RepositoryInjector.Get<IAccommodationReservationCancellationNotificationRepository>().
                                    GetByOwnerId(ownerId);
            var cancelledReservatons = new List<AccommodationReservation>();
            notifactions.ForEach(n => cancelledReservatons.Add(
                    _reservations.Find(r => r.Id == n.ReservationId)
                )
            );
            return cancelledReservatons;
        }
        public void MoveReservation(int reservationId, DateOnly newCheckIn, DateOnly NewCheckOut)
        {
            GetAll();
            AccommodationReservation reservation = new AccommodationReservation();
            AccommodationReservation newReservation = new AccommodationReservation();
            reservation = _reservations.Find(r => r.Id == reservationId);
            newReservation = reservation;
            newReservation.CheckIn = newCheckIn;
            newReservation.CheckOut = NewCheckOut;
            _reservations.Remove(reservation);
            _reservations.Add(newReservation);
            _fileHandler.Save(_reservations);
        }

        public string CheckAvailability(int accomodationId, DateOnly checkIn, DateOnly checkOut)
        {
            GetAll();
            foreach (AccommodationReservation res in _reservations)
            {
                var AccommodationIsUnavailable = (checkIn > res.CheckIn 
                    || checkIn < res.CheckOut 
                    || checkOut > res.CheckIn 
                    || checkOut < res.CheckOut) 
                    && accomodationId == res.Accommodation.Id;

                if (AccommodationIsUnavailable)
                {
                    return "Unavailable";
                }
            }
            return "Available";
        }
    }
}
