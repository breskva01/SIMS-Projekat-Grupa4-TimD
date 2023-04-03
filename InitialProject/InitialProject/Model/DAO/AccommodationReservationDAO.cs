using InitialProject.FileHandler;
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
        public List<AccommodationReservation> GetConfirmed(int guestId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.GuestId == guestId && r.Status == AccommodationReservationStatus.Confirmed);
        }
        public void Cancel(int reservationId)
        {
            _reservations = _fileHandler.Load();
            _reservations.Find(r => r.Id == reservationId).Status = AccommodationReservationStatus.Cancelled;
            _fileHandler.Save(_reservations);
        }
        public List<AccommodationReservation> FindAvailable(DateOnly beginDate, DateOnly endDate, int days, Accommodation accommodation, User guest)
        {
            var existingReservations = FindExisting(accommodation.Id);

            var availableReservations = FindInsideDateFrame(beginDate, endDate, days, accommodation,
                                                              guest, existingReservations);
            if (availableReservations.Count == 0)
                availableReservations = FindOutsideDateFrame(beginDate, endDate, days, accommodation,
                                                             guest, existingReservations);
            return availableReservations;
        }
        private List<AccommodationReservation> FindInsideDateFrame(DateOnly beginDate, DateOnly endDate, int days, Accommodation accommodation, User guest, List<AccommodationReservation> existingReservations)
        {
            List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
            DateOnly checkIn = beginDate;
            DateOnly checkOut = beginDate.AddDays(days);
            int numberOfReservations = 0;

            while (numberOfReservations < 3 && checkOut <= endDate)
            {
                if (IsAvailable(checkIn, checkOut, existingReservations))
                {
                    availableReservations.Add(CreateReservation(accommodation, guest, days, checkIn, checkOut));
                    numberOfReservations++;
                }
                checkIn = checkIn.AddDays(1);
                checkOut = checkOut.AddDays(1);
            }
            return availableReservations;
        }
        private List<AccommodationReservation> FindOutsideDateFrame(DateOnly beginDate, DateOnly endDate, int days, Accommodation accommodation, User guest, List<AccommodationReservation> existingReservations)
        {
            List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
            DateOnly checkIn = FindStartingDates(ref beginDate, ref endDate, existingReservations, days);
            DateOnly checkOut = checkIn.AddDays(days);
            bool beforeDateFrame = false;
            int offset = 0;
            int numberOfReservations = 0;

            while (numberOfReservations < 3)
            {
                if (IsAvailable(checkIn, checkOut, existingReservations))
                {
                    availableReservations.Add(CreateReservation(accommodation, guest, days, checkIn, checkOut));
                    numberOfReservations++;
                }
                if (beforeDateFrame)
                {
                    checkIn = beginDate.AddDays(-offset);
                    checkOut = checkIn.AddDays(days);
                    beforeDateFrame = !beforeDateFrame;
                }
                else
                {
                    checkIn = endDate.AddDays(offset);
                    checkOut = checkIn.AddDays(days);
                    offset++;
                    beforeDateFrame = !beforeDateFrame;
                }
            }
            return availableReservations;
        }
        private DateOnly FindStartingDates(ref DateOnly beginDate, ref DateOnly endDate, List<AccommodationReservation> existingReservations, int days)
        {
            try
            {
                var localBeginDate = beginDate;
                var localEndDate = endDate;
                var reservationsInsideTimeFrime = existingReservations.FindAll(r => r.CheckIn >= localBeginDate 
                                                                               && r.CheckOut <= localEndDate);
                beginDate = reservationsInsideTimeFrime.Min(r => r.CheckIn).AddDays(-days);
                endDate = reservationsInsideTimeFrime.Max(r => r.CheckOut);
            }
            catch
            {
                endDate = beginDate.AddDays(1);
            }
            return beginDate;           
        }
        private bool IsAvailable(DateOnly checkIn, DateOnly checkOut, List<AccommodationReservation> existingReservations)
        {
            if (checkIn < DateOnly.FromDateTime(DateTime.Now))
                return false;
            foreach(var reservation in existingReservations)
            {
                if (reservation.Overlaps(checkIn, checkOut))
                    return false;
            }
            return true;
        }
        public List<AccommodationReservation> FindExisting(int accommodationId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.AccommodationId == accommodationId &&
                                         r.Status == AccommodationReservationStatus.Confirmed);
        }
        private AccommodationReservation CreateReservation(Accommodation accommodation, User guest, int days, DateOnly checkIn, DateOnly checkOut)
        {
            return new AccommodationReservation(accommodation, guest, days, checkIn, checkOut, AccommodationReservationStatus.Confirmed);
        }
        public void Save(AccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _reservations = _fileHandler.Load();
            _reservations.Add(reservation);
            _fileHandler.Save(_reservations);
        }

        public bool IsCompleted(AccommodationReservation accommodationReservation, int ownerId)
        {
            return (accommodationReservation.CheckOut < DateOnly.FromDateTime(DateTime.Now)) 
                    && (DateOnly.FromDateTime(DateTime.Now) < (accommodationReservation.CheckOut.AddDays(5))) 
                    && accommodationReservation.Accommodation.OwnerId == ownerId;
        }

        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId)
        {
            List<AccommodationReservation> completedReservations= new List<AccommodationReservation>();
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
