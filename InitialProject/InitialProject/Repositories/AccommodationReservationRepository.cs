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

namespace InitialProject.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository, ISubject
    {
        private readonly List<IObserver> _observers;
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;
        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            _reservations = _fileHandler.Load();
            _observers = new List<IObserver>();
        }
        public List<AccommodationReservation> GetAll()
        {
            return _fileHandler.Load();
        }
        public AccommodationReservation GetById(int reservationId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.Find(r => r.Id == reservationId);
        }
        public List<AccommodationReservation> GetConfirmed(int guestId)
        {
            _reservations = _fileHandler.Load();
            var existingReservations = _reservations.FindAll(r => r.GuestId == guestId &&
                                         r.Status == AccommodationReservationStatus.Confirmed);
            return existingReservations.OrderBy(r => r.CheckIn).ToList();
        }
        public List<AccommodationReservation> GetExisting(int accommodationId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.AccommodationId == accommodationId &&
                                         r.Status == AccommodationReservationStatus.Confirmed);
        }
        public List<AccommodationReservation> GetExistingInsideDateRange(int accommodationId, DateOnly startDate, DateOnly endDate)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.AccommodationId == accommodationId &&
                                         r.Status == AccommodationReservationStatus.Confirmed &&
                                         r.CheckIn >= startDate && r.CheckOut <= endDate);
        }
        public void Cancel(int reservationId)
        {
            var reservation = GetById(reservationId);
            reservation.Status = AccommodationReservationStatus.Cancelled;
            _fileHandler.Save(_reservations);
        }
        public bool IsCompleted(AccommodationReservation accommodationReservation, int ownerId)
        {
            return accommodationReservation.CheckOut < DateOnly.FromDateTime(DateTime.Now)
                    && DateOnly.FromDateTime(DateTime.Now) < accommodationReservation.CheckOut.AddDays(5)
                    && accommodationReservation.Accommodation.OwnerId == ownerId;
        }

        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId)
        {
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
            AccommodationReservation newAccommodationReservation = new AccommodationReservation();
            newAccommodationReservation = accommodationReservation;
            newAccommodationReservation.LastNotification = newAccommodationReservation.LastNotification.AddDays(1);
            _reservations.Remove(accommodationReservation);
            _reservations.Add(newAccommodationReservation);
            _fileHandler.Save(_reservations);
            NotifyObservers();
        }
        public void updateRatingStatus(AccommodationReservation accommodationReservation)
        {
            _reservations = _fileHandler.Load();
            AccommodationReservation newAccommodationReservation = _reservations.Find(a => a.Id == accommodationReservation.Id);
            newAccommodationReservation.IsGuestRated = true;
            _fileHandler.Save(_reservations);
            NotifyObservers();
        }
        public void Save(AccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _reservations = _fileHandler.Load();
            _reservations.Add(reservation);
            _fileHandler.Save(_reservations);
        }
        private int NextId()
        {
            _reservations = _fileHandler.Load();
            return _reservations?.Max(r => r.Id) + 1 ?? 0;
            //return _reservations.Count == 0 ? 0 : _reservations.Max(r => r.Id) + 1;
        }

        public List<AccommodationReservation> GetEgligibleForRating(int guestId)
        {
            _reservations = _fileHandler.Load();
            DateOnly fiveDaysAgo = DateOnly.FromDateTime(DateTime.Now.AddDays(-5));
            var egligibleReservations = _reservations.FindAll(r => r.GuestId == guestId &&
                                         r.Status == AccommodationReservationStatus.Finished &&
                                         r.IsOwnerRated == false &&
                                         r.CheckOut >= fiveDaysAgo);
            return egligibleReservations.OrderBy(r => r.CheckIn).ToList();
        }

        public void MarkOwnerAsRated(int reservationId)
        {
            var reservation = GetById(reservationId);
            reservation.IsOwnerRated = true;
            _fileHandler.Save(_reservations);
        }

        public List<AccommodationReservation> GetAllNewlyCancelled(int ownerId)
        {
            _reservations = _fileHandler.Load();
            var notifactions = RepositoryStore.GetIAccommodationReservationCancellationNotificationRepository.
                                               GetByOwnerId(ownerId);
            var cancelledReservatons = new List<AccommodationReservation>();
            notifactions.ForEach(n =>
                cancelledReservatons.Add(
                    _reservations.Find(r => r.Id == n.ReservationId)
                )
            );
            return cancelledReservatons;
        }
        public void MoveReservation(int reservationId, DateOnly newCheckIn, DateOnly NewCheckOut)
        {
            _reservations = _fileHandler.Load();
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
            _reservations = _fileHandler.Load();
            foreach (AccommodationReservation res in _reservations)
            {
                var AccommodationIsUnavailable = (checkIn > res.CheckIn 
                    || checkIn < res.CheckOut 
                    || checkOut > res.CheckIn 
                    || checkOut < res.CheckOut) 
                    && accomodationId == res.AccommodationId;

                if (AccommodationIsUnavailable)
                {
                    return "Unavailable";
                }
            }
            return "Available";
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
