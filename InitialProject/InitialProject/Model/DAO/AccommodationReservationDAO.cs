﻿using InitialProject.FileHandler;
using InitialProject.Observer;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Model.DAO
{
    public class AccommodationReservationDAO : ISubject
    {
        private readonly List<IObserver> _observers;
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;

        public AccommodationReservationDAO()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            _reservations = _fileHandler.Load();
            _observers = new List<IObserver>();
        }
        public List<AccommodationReservation> GetAll()
        {
            return _fileHandler.Load();
        }
        public List<AccommodationReservation> FindAvailable(DateOnly beginDate, DateOnly endDate, int days, Accommodation accommodation, User guest)
        {
            List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
            List<AccommodationReservation> existingReservations = FindExisting(accommodation);

            int counter = 0;
            DateOnly checkIn = beginDate;
            DateOnly checkOut = beginDate.AddDays(days);
            while (counter < 3 && checkOut < endDate)
            {
                if (IsAvailable(checkIn, checkOut, existingReservations)) 
                {
                    AccommodationReservation reservation = CreateReservation(accommodation, guest, days, checkIn, checkOut);
                    availableReservations.Add(reservation);
                    counter++;
                }
                checkIn = checkIn.AddDays(1);
                checkOut = checkOut.AddDays(1);
            }
            return availableReservations;
        }
        public bool IsAvailable(DateOnly checkIn, DateOnly checkOut, List<AccommodationReservation> reservations)
        {
            foreach(var reservation in reservations)
            {
                if (reservation.Overlap(checkIn, checkOut))
                    return false;
            }
            return true;
        }
        public List<AccommodationReservation> FindExisting(Accommodation accommodation)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(a => a.Accommodation == accommodation);
        }
        private AccommodationReservation CreateReservation(Accommodation accommodation, User guest, int days, DateOnly checkIn, DateOnly checkOut)
        {
            AccommodationReservation reservation = new AccommodationReservation(accommodation, guest, days, checkIn, checkOut);
            reservation.Id = NextId();
            return reservation;
        }

        public bool IsCompleted(AccommodationReservation accommodationReservation, int ownerId)
        {
            return (accommodationReservation.CheckOutDate < DateOnly.FromDateTime(DateTime.Now)) 
                    && (DateOnly.FromDateTime(DateTime.Now) < (accommodationReservation.CheckOutDate.AddDays(5))) 
                    && accommodationReservation.Accommodation.OwnerId == ownerId;
        }

        public List<AccommodationReservation> FindCompletedAndUnratedReservations(int ownerId)
        {
            List<AccommodationReservation> completedReservations= new List<AccommodationReservation>();
            foreach (AccommodationReservation accommodationReservation in _reservations)
            {
                if (IsCompleted(accommodationReservation, ownerId) && !accommodationReservation.IsGuestRated)
                {
                    completedReservations.Add(accommodationReservation);
                }
            }
            return completedReservations;
        }
        /*public void updateLastNotification(AccommodationReservation accommodationReservation)
        {
            AccommodationReservation newAccommodationReservation= new AccommodationReservation();
            newAccommodationReservation = accommodationReservation;
            newAccommodationReservation.LastNotification++;
            _reservations.Remove(accommodationReservation);
            _reservations.Add(newAccommodationReservation);
            _fileHandler.Save(_reservations);
            NotifyObservers();
        }
        public void updateRatingStatus(AccommodationReservation accommodationReservation)
        {
            AccommodationReservation newAccommodationReservation = new AccommodationReservation();
            newAccommodationReservation = accommodationReservation;
            newAccommodationReservation.IsGuestRated = true;
            _reservations.Remove(accommodationReservation);
            _reservations.Add(newAccommodationReservation);
            _fileHandler.Save(_reservations);
            NotifyObservers();
        }*/

        public int NextId()
        {
            _reservations = _fileHandler.Load();
            if (_reservations.Count < 1)
            {
                return 1;
            }
            return _reservations.Max(r => r.Id) + 1;
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
