using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourFileHandler _tourFileHandler;
        private readonly LocationFileHandler _locationFileHandler;
        private readonly UserFileHandler _userFileHandler;
        private readonly TourReservationFileHandler _tourReservationFileHandler;
        private List<Tour> _tours;
        private List<Location> _locations;

        public TourRepository()
        {
            _tourFileHandler = new TourFileHandler();
            _locationFileHandler = new LocationFileHandler();
            _userFileHandler = new UserFileHandler();
            _tourReservationFileHandler = new TourReservationFileHandler();
            _tours = _tourFileHandler.Load();
        }

        public List<Tour> GetAll()
        {
            return _tourFileHandler.Load();
        }
        public Tour GetById(int tourId)
        {
            _tours = _tourFileHandler.Load();
            return _tours.Find(v => v.Id == tourId);
        }
        public Tour GetByName(String name)
        {
            List<Tour> tours = _tourFileHandler.Load();
            foreach (Tour tour in tours)
            {
                if (tour.Name == name)
                {
                    return tour;
                    break;
                }
            }
            return null;
        }
        public Tour Update(Tour tour)
        {
            _tours = _tourFileHandler.Load();
            Tour updated = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(updated);
            _tours.Add(tour);
            _tourFileHandler.Save(_tours);
            return tour;
        }
        public void Delete(Tour tour)
        {
            _tours = _tourFileHandler.Load();
            Tour founded = _tours.Find(a => a.Id == tour.Id);
            _tours.Remove(founded);
            _tourFileHandler.Save(_tours);
        }
        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _tourFileHandler.Load();
            _tours.Add(tour);
            _tourFileHandler.Save(_tours);
            return tour;
        }
        public int NextId()
        {
            _tours = _tourFileHandler.Load();
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }
        public bool IsActive(int tourId)
        {
            return GetById(tourId).State == TourState.Started;
        }
        public List<Tour> GetFiltered(string country, string city, int duration, GuideLanguage language, int numberOfGuests)
        {
            _tours = _tourFileHandler.Load();
            _locations = _locationFileHandler.Load();

            foreach (Tour t in _tours)
            {
                t.Location = _locations.FirstOrDefault(l => l.Id == t.LocationId);
            }

            List<Tour> filteredTours = new();
            foreach (Tour tour in _tours)
            {
                if (MatchesFilters(tour, country, city, duration, language, numberOfGuests))
                {
                    filteredTours.Add(tour);
                }
            }
            return filteredTours;
        }
        public bool MatchesFilters(Tour tour, string country, string city, int duration, GuideLanguage language, int numberOfGuests)
        {
            bool countryMatch = tour.Location.Country == country || country == "";
            bool cityMatch = tour.Location.City == city || city == "";
            bool durationMatch = tour.Duration == duration || duration == 0;
            bool languageMatch = tour.Language == language || language == GuideLanguage.All;
            bool numberOfGuestsMatch = tour.MaximumGuests - tour.CurrentNumberOfGuests >= numberOfGuests || numberOfGuests == 0;

            return countryMatch && cityMatch && durationMatch && languageMatch && numberOfGuestsMatch;
        }
        public List<Tour> SortByName(List<Tour> tours)
        {
            return tours.OrderBy(t => t.Name).ToList();
        }
        public List<Tour> SortByLocation(List<Tour> tours)
        {
            return tours.OrderBy(t => t.Location.Country).ThenBy(t => t.Location.City).ToList();
        }
        public List<Tour> SortByDuration(List<Tour> tours)
        {
            return tours.OrderBy(t => t.Duration).ToList();
        }
        public List<Tour> SortByLanguage(List<Tour> tours)
        {
            return tours.OrderBy(t => t.Language).ToList();
        }
        public Tour GetMostVisited(String selectedYear)
        {
            List<Tour> tours = new List<Tour>();
            Tour mostVisited = new Tour();

            if (selectedYear == "All time")
            {
                tours = _tourFileHandler.Load();
                mostVisited = tours[0];
                foreach (Tour tour in tours)
                {
                    if (tour.NumberOfArrivedGeusts > mostVisited.NumberOfArrivedGeusts && tour.State == TourState.Finished)
                    {
                        mostVisited = tour;
                    }
                }
                return mostVisited;
            }
            tours = GetToursByYear(selectedYear);
            mostVisited = tours[0];
            foreach (Tour tour in tours)
            {
                if (tour.NumberOfArrivedGeusts > mostVisited.NumberOfArrivedGeusts)
                {
                    mostVisited = tour;
                }
            }
            return mostVisited;
        }
        public List<Tour> GetToursByYear(String year)
        {
            List<Tour> tours = new List<Tour>();
            tours = _tourFileHandler.Load();
            List<Tour> filtered = new List<Tour>();
            foreach(Tour tour in tours)
            {
                if(tour.Start.Year.ToString() == year)
                {
                    filtered.Add(tour);
                }
            }
            return filtered;
        }
        public List<string> GetAvailableYears()
        {
            List<string> years = new List<string>();
            List<Tour> tours = _tourFileHandler.Load();
            foreach(Tour tour in tours)
            {
                if(!years.Contains(tour.Start.Year.ToString()))
                {
                    years.Add(tour.Start.Year.ToString());
                }
            }
            return years;
        }
        public List<string> GetFinishedTourNames()
        {
            List<Tour> tours = _tourFileHandler.Load();
            List<string> names = new List<string>();
            foreach(Tour tour in tours)
            {
                if(tour.State == TourState.Finished)
                {
                    names.Add(tour.Name);
                }
            }
            return names;
        }
        public string GetNumberOfGuestBelow18(Tour tour)
        {
            int number = 0;
            List<User> guests = _userFileHandler.Load();
            List<TourReservation> reservations = _tourReservationFileHandler.Load();

            foreach (TourReservation tourReservation in reservations)
            {
                foreach( User guest in guests)
                {
                    if(tourReservation.GuestId == guest.Id && tourReservation.TourId == tour.Id && guest.Age < 18)
                    {
                        number += tourReservation.NumberOfGuests;
                    }
                }
            }
            return number.ToString();
        }
        public string GetNumberOfMiddleAgeGuests(Tour tour)
        {
            int number = 0;
            List<User> guests = _userFileHandler.Load();
            List<TourReservation> reservations = _tourReservationFileHandler.Load();

            foreach (TourReservation tourReservation in reservations)
            {
                foreach (User guest in guests)
                {
                    if (tourReservation.GuestId == guest.Id && tourReservation.TourId == tour.Id && guest.Age >= 18 && guest.Age < 50)
                    {
                        number += tourReservation.NumberOfGuests;
                    }
                }
            }
            return number.ToString();
        }
        public string GetNumberOfOlderGuests(Tour tour)
        {
            int number = 0;
            List<User> guests = _userFileHandler.Load();
            List<TourReservation> reservations = _tourReservationFileHandler.Load();

            foreach (TourReservation tourReservation in reservations)
            {
                foreach (User guest in guests)
                {
                    if (tourReservation.GuestId == guest.Id && tourReservation.TourId == tour.Id && guest.Age > 50)
                    {
                        number += tourReservation.NumberOfGuests;
                    }
                }
            }
            return number.ToString();
        }
        public List<Tour> GetFinishedTours()
        {
            _tours = _tourFileHandler.Load();
            List<Tour> filterdTours = new List<Tour>();
            foreach(Tour tour in _tours)
            {
                if(tour.State == TourState.Finished)
                {
                    filterdTours.Add(tour);
                }
            }
            return filterdTours;
        }
    }
}
