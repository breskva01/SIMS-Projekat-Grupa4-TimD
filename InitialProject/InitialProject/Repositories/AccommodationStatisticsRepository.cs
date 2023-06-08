﻿using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationStatisticsRepository : IAccommodationStatisticsRepository
    {
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;
        private AccommodationReservationMoveRequestRepository _moveRequestRepository;
        private AccommodationRatingRepository _ratingRepository;
        private LocationRepository _locationRepository;
        public AccommodationStatisticsRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            _moveRequestRepository = new AccommodationReservationMoveRequestRepository();
            _ratingRepository = new AccommodationRatingRepository();
            _locationRepository = new LocationRepository();
        }
        public List<AccommodationReservation> GetAllReservations()
        {
            return _reservations = _fileHandler.Load();
        }
        public LineSeries GetYearlyReservations(int id)
        {
            GetAllReservations();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> reservationsPerYear = new List<AccommodationReservation>();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    if (res.Accommodation.Id == id && res.CheckIn.Year == i)
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
            GetAllReservations();
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
            GetAllReservations();
            var series = new LineSeries();
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> movedReservationsPerYear = new List<AccommodationReservation>();
            List<AccommodationReservationMoveRequest> requests = _moveRequestRepository.GetAll();
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    foreach (AccommodationReservationMoveRequest request in requests)
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
            GetAllReservations();
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
                        if (res.Accommodation.Id == id && res.CheckIn.Year == i && res.Id == rating.Reservation.Id && rating.RenovationUrgency > 0)
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
            GetAllReservations();
            var series = new LineSeries();
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> reservationsPerMonth = new List<AccommodationReservation>();
            for (int i = start.Month; i <= end.Month; i++)
            {
                foreach (AccommodationReservation res in _reservations)
                {
                    if (res.Accommodation.Id == id && start.Year == res.CheckIn.Year && res.CheckIn.Month == i)
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
            GetAllReservations();
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
            GetAllReservations();
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
            GetAllReservations();
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
            GetAllReservations();
            string mostBooked;
            int sumOfDays = 0;
            int max = 0;
            DateOnly start = new DateOnly(2020, 1, 1);
            List<AccommodationReservation> fileteredReservations = new List<AccommodationReservation>();
            List<int> occupiedDays = new List<int>();
            foreach (AccommodationReservation res in _reservations)
            {
                if (res.Accommodation.Id == id)
                    fileteredReservations.Add(res);
            }
            for (int i = start.Year; i <= DateTime.Now.Year; i++)
            {
                foreach (AccommodationReservation res in fileteredReservations)
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
                if (sum > max)
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
            GetAllReservations();
            string mostBooked;
            int monthNumber;
            int sumOfDays = 0;
            int max = 0;
            DateOnly start = new DateOnly(year, 1, 1);
            DateOnly end = new DateOnly(year, 12, 31);
            List<AccommodationReservation> filteredReservations = new List<AccommodationReservation>();
            List<int> occupiedDays = new List<int>();
            foreach (AccommodationReservation res in _reservations)
            {
                if (res.CheckIn.Year == year && res.Accommodation.Id == id)
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
        public List<Location> GetMostPopularLocations() 
        {
            GetAllReservations();
            List<Location> locations= new List<Location>();
            List<LocationPopularity> popularities= new List<LocationPopularity>();
            foreach(AccommodationReservation reservation in _reservations)
            {
                if (locations.Find(l => l.Id == reservation.Accommodation.Location.Id) == null)
                {
                    locations.Add(reservation.Accommodation.Location);
                }
            }
            foreach(Location location in locations)
            {
                LocationPopularity locationPopularity = new LocationPopularity(location, 0, 0);
                popularities.Add(locationPopularity);
            }
            locations.Clear();
            int totalReservations = 0;
            int sumOfDays = 0;
            DateTime start = new DateTime(2020, 1, 1);
            TimeSpan duration;
            foreach (LocationPopularity popularity in popularities)
            {
                foreach(AccommodationReservation reservation in _reservations)
                {
                    if(reservation.Accommodation.Location.Id == popularity.Location.Id)
                    {
                        totalReservations += 1;
                        sumOfDays += reservation.CheckOut.DayNumber - reservation.CheckIn.DayNumber;
                    }
                }
                popularity.Reservations = totalReservations;
                duration = DateTime.Now - start;
                popularity.OccupancyRate = sumOfDays / Convert.ToDouble(duration.Days);
                totalReservations = 0;
                sumOfDays = 0;
            }
            var orderedPopularities = popularities.OrderBy(p => p.Reservations).ThenBy(p => p.OccupancyRate);
            var reversedPopularities = orderedPopularities.Reverse();
            int i = 0;
            foreach(LocationPopularity popularity in reversedPopularities)
            {
                locations.Add(popularity.Location);
                i += 1;
                if(i == 3)
                {
                    break;
                }
            }
            return locations;
        }
        public List<Location> GetMostUnpopularLocations()
        {
            GetAllReservations();
            List<Location> locations = new List<Location>();
            List<LocationPopularity> popularities = new List<LocationPopularity>();
            foreach (AccommodationReservation reservation in _reservations)
            {
                if (locations.Find(l => l.Id == reservation.Accommodation.Location.Id) == null)
                {
                    locations.Add(reservation.Accommodation.Location);
                }
            }
            foreach (Location location in locations)
            {
                LocationPopularity locationPopularity = new LocationPopularity(location, 0, 0);
                popularities.Add(locationPopularity);
            }
            locations.Clear();
            int totalReservations = 0;
            int sumOfDays = 0;
            DateTime start = new DateTime(2020, 1, 1);
            TimeSpan duration;
            foreach (LocationPopularity popularity in popularities)
            {
                foreach (AccommodationReservation reservation in _reservations)
                {
                    if (reservation.Accommodation.Location.Id == popularity.Location.Id)
                    {
                        totalReservations += 1;
                        sumOfDays += reservation.CheckOut.DayNumber - reservation.CheckIn.DayNumber;
                    }
                }
                popularity.Reservations = totalReservations;
                duration = DateTime.Now - start;
                popularity.OccupancyRate = sumOfDays / Convert.ToDouble(duration.Days);
                totalReservations = 0;
                sumOfDays = 0;
            }
            var orderedPopularities = popularities.OrderBy(p => p.Reservations).ThenBy(p => p.OccupancyRate);
            int i = 0;
            foreach (LocationPopularity popularity in orderedPopularities)
            {
                locations.Add(popularity.Location);
                i += 1;
                if (i == 3)
                {
                    break;
                }
            }
            return locations;
        }
    }
}
