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
        public List<AccommodationReservation> GetFilteredReservations(int? guestId = null, int? accommodationId = null,
            DateOnly? startDate = null, DateOnly? endDate = null,
            AccommodationReservationStatus status = AccommodationReservationStatus.Active)
        {
            GetAll();
            var reservations = _reservations.Where(r => r.Status == status);

            if (guestId.HasValue)
                reservations = reservations.Where(r => r.Guest.Id == guestId.Value);

            if (accommodationId.HasValue)
                reservations = reservations.Where(r => r.Accommodation.Id == accommodationId.Value);

            if (startDate.HasValue)
                reservations = reservations.Where(r => r.CheckIn >= startDate.Value);

            if (endDate.HasValue)
                reservations = reservations.Where(r => r.CheckOut <= endDate.Value);

            return reservations.OrderBy(r => r.CheckIn).ToList();
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

        public void MarkOwnerAsRated(int reservationId)
        {
            var reservation = GetById(reservationId);
            reservation.IsOwnerRated = true;
            _fileHandler.Save(_reservations);
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
