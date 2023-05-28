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
using OxyPlot.Series;
using OxyPlot;
using System.Security.Cryptography;
using System.Collections;

namespace InitialProject.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;
        private AccommodationReservationMoveRequestRepository _moveRequestRepository;
        private AccommodationRatingRepository _ratingRepository; 
        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            _moveRequestRepository = new AccommodationReservationMoveRequestRepository();
            _ratingRepository = new AccommodationRatingRepository();
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
        public List<TimeSlot> GetAvailableDates(DateTime start, DateTime end, int duration, int id)
        {
            GetAll();
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            
            for(DateTime begin = start; begin <= end.AddDays(-duration); begin=begin.AddDays(1))
            {
                TimeSlot timeSlot = new TimeSlot();
                timeSlot.Start = begin;
                timeSlot.End = begin.AddDays(duration);
                timeSlots.Add(timeSlot);
            }
            foreach(AccommodationReservation res in _reservations)
            {
                DateTime CheckIn = new DateTime(res.CheckIn.Year, res.CheckIn.Month, res.CheckIn.Day);
                DateTime CheckOut= new DateTime(res.CheckOut.Year, res.CheckOut.Month, res.CheckOut.Day); ;
                foreach (TimeSlot slot in timeSlots.ToList())
                {
                    if (res.Accommodation.Id == id && res.Status == AccommodationReservationStatus.Active && ((slot.Start>=CheckIn.AddDays(-duration) && slot.Start<=CheckOut)))
                    {
                        timeSlots.Remove(slot);
                    }
                }
            }
            return timeSlots;
        }
        public void Update(AccommodationReservation reservation)
        {
            GetAll();
            AccommodationReservation oldReservation = _reservations.Find(r => r.Id == reservation.Id);
            _reservations.Remove(oldReservation);
            _reservations.Add(reservation);
            _fileHandler.Save(_reservations);
        }
    }
}
