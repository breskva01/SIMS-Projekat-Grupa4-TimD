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
        public void updateLastNotification(AccommodationReservation oldAccommodationReservation)
        {
            GetAll();
            AccommodationReservation newAccommodationReservation = new AccommodationReservation();
            newAccommodationReservation = oldAccommodationReservation;
            newAccommodationReservation.LastNotification = newAccommodationReservation.LastNotification.AddDays(1);
            foreach(AccommodationReservation res in _reservations.ToList())
            {
                if(res.Id == oldAccommodationReservation.Id)
                {
                    _reservations.Remove(res);
                }
            }
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
        public LineSeries GetYearlyReservations(int id)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> reservationsPerYear = new List<AccommodationReservation>();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach(AccommodationReservation res in _reservations)
                {
                    if(res.Accommodation.Id==id && res.CheckIn.Year==i)
                    {
                        reservationsPerYear.Add(res);
                    }
                }
                series.Points.Add(new DataPoint(i, reservationsPerYear.Count));
                reservationsPerYear.Clear();
            }
            return series;
        }
        public LineSeries GetYearlyCancellations(int id)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> cancellationsPerYear = new List<AccommodationReservation>();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    if (res.Accommodation.Id == id && res.CheckIn.Year == i && res.Status == AccommodationReservationStatus.Cancelled)
                    {
                        cancellationsPerYear.Add(res);
                    }
                }
                series.Points.Add(new DataPoint(i, cancellationsPerYear.Count));
                cancellationsPerYear.Clear();
            }
            return series;
        }
        public LineSeries GetYearlyMovedReservations(int id)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> movedReservationsPerYear = new List<AccommodationReservation>();
            List<AccommodationReservationMoveRequest> requests = _moveRequestRepository.GetAll();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    foreach(AccommodationReservationMoveRequest request in requests)
                    {
                        if (res.Accommodation.Id == id && res.CheckIn.Year == i && res.Id == request.Reservation.Id && request.Status == ReservationMoveRequestStatus.Accepted)
                        {
                            movedReservationsPerYear.Add(res);
                        }
                    }
                }
                series.Points.Add(new DataPoint(i, movedReservationsPerYear.Count));
                movedReservationsPerYear.Clear();
            }
            return series;
        }
        public LineSeries GetYearlyRenovationReccommendations(int id) 
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> renovationReccommendationsPerYear = new List<AccommodationReservation>();
            List<AccommodationRating> ratings = _ratingRepository.GetAll();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    foreach (AccommodationRating rating in ratings)
                    {
                        if (res.Accommodation.Id == id && res.CheckIn.Year == i && res.Id == rating.Reservation.Id && rating.RenovationUrgency>0)
                        {
                            renovationReccommendationsPerYear.Add(res);
                        }
                    }
                }
                series.Points.Add(new DataPoint(i, renovationReccommendationsPerYear.Count));
                renovationReccommendationsPerYear.Clear();
            }
            return series;
        }
        public LineSeries GetMonthlyReservations(int id, int year)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> reservationsPerMonth= new List<AccommodationReservation>();
            for(int i = start.Month; i <= end.Month; i++)
            {
                foreach(AccommodationReservation res in _reservations)
                {
                    if(res.Accommodation.Id == id && start.Year==res.CheckIn.Year && res.CheckIn.Month == i)
                    {
                        reservationsPerMonth.Add(res);
                    }
                }
                series.Points.Add(new DataPoint(i, reservationsPerMonth.Count));
                reservationsPerMonth.Clear();
            }
            return series;
        }
        public LineSeries GetMonthlyCancellations(int id, int year)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> cancellationsPerMonth = new List<AccommodationReservation>();
            for (int i = start.Month; i <= end.Month; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    if (res.Accommodation.Id == id && start.Year == res.CheckIn.Year && res.CheckIn.Month == i && res.Status == AccommodationReservationStatus.Cancelled)
                    {
                        cancellationsPerMonth.Add(res);
                    }
                }
                series.Points.Add(new DataPoint(i, cancellationsPerMonth.Count));
                cancellationsPerMonth.Clear();
            }
            return series;
        }
        public LineSeries GetMonthlyMovedReservations(int id, int year)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> movedReservationsPerMonth = new List<AccommodationReservation>();
            List<AccommodationReservationMoveRequest> requests = _moveRequestRepository.GetAll();
            for (int i = start.Month; i <= end.Month; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    foreach (AccommodationReservationMoveRequest request in requests)
                    {
                        if (res.Accommodation.Id == id && start.Year == res.CheckIn.Year && res.CheckIn.Month == i && res.Id == request.Reservation.Id && request.Status == ReservationMoveRequestStatus.Accepted)
                        {
                            movedReservationsPerMonth.Add(res);
                        }
                    }
                }
                series.Points.Add(new DataPoint(i, movedReservationsPerMonth.Count));
                movedReservationsPerMonth.Clear();
            }
            return series;
        }
        public LineSeries GetMonthlyRenovationReccommendations(int id, int year)
        {
            GetAll();
            var series = new LineSeries();
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> renovationReccommendationsPerMonth = new List<AccommodationReservation>();
            List<AccommodationRating> ratings = _ratingRepository.GetAll();
            for (int i = start.Month; i <= end.Month; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    foreach (AccommodationRating rating in ratings)
                    {
                        if (res.Accommodation.Id == id && res.CheckIn.Month == i && res.Id == rating.Reservation.Id && rating.RenovationUrgency > 0)
                        {
                            renovationReccommendationsPerMonth.Add(res);
                        }
                    }
                }
                series.Points.Add(new DataPoint(i, renovationReccommendationsPerMonth.Count));
                renovationReccommendationsPerMonth.Clear();
            }
            return series;
        }
        public string GetMostBookedYear(int id)
        {
            GetAll();
            string mostBooked;
            int sumOfDays=0;
            int max=0;
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> fileteredReservations = new List<AccommodationReservation>();
            List<int> occupiedDays= new List<int>();
            foreach(AccommodationReservation res in _reservations)
            {
                if(res.Accommodation.Id == id)
                    fileteredReservations.Add(res); 
            }
            for(int i=start.Year; i<=DateTime.Now.Year; i++)
            {
                foreach(AccommodationReservation res in fileteredReservations)
                {
                    if (res.CheckIn.Year == i)
                        sumOfDays += res.CheckOut.Day - res.CheckIn.Day;
                }
                occupiedDays.Add(sumOfDays);
                sumOfDays = 0;
            }
            mostBooked = start.Year.ToString();
            foreach (int sum in occupiedDays)
            {
                if(sum > max)
                {
                    max = sum;
                    mostBooked = start.Year.ToString();
                }
                start = start.AddYears(1);
            }
            return mostBooked;
        }
        public string GetMostBookedMonth(int id, int year)
        {
            GetAll();
            string mostBooked;
            int monthNumber;
            int sumOfDays = 0;
            int max = 0;
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> filteredReservations= new List<AccommodationReservation>();
            List<int> occupiedDays = new List<int>();
            foreach (AccommodationReservation res in _reservations)
            {
                if(res.CheckIn.Year == year && res.Accommodation.Id == id)
                {
                    filteredReservations.Add(res);
                }
            }
            for (int i = start.Month; i <= end.Month; i++)
            {
                foreach (AccommodationReservation res in filteredReservations)
                {
                    if (res.CheckIn.Month == i)
                        sumOfDays += res.CheckOut.Day - res.CheckIn.Day;
                }
                occupiedDays.Add(sumOfDays);
                sumOfDays = 0;
            }
            monthNumber = start.Month;
            foreach (int sum in occupiedDays)
            {
                if (sum > max)
                {
                    max = sum;
                    monthNumber = start.Month;
                }
                start = start.AddMonths(1);
            }
            mostBooked = GetMonthName(monthNumber);
            return mostBooked;
        }
        private string GetMonthName(int monthNumber)
        {
            switch (monthNumber) 
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                default:
                    return "December";
            }
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
    }
}
