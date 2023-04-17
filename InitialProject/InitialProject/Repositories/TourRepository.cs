using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class TourRepository
    {
        private readonly TourFileHandler _tourFileHandler;
        private readonly LocationFileHandler _locationFileHandler;
        private List<Tour> _tours;
        private List<Location> _locations;

        public TourRepository()
        {
            _tourFileHandler = new TourFileHandler();
            _locationFileHandler = new LocationFileHandler();
            _tours = _tourFileHandler.Load();
        }

        public List<Tour> GetAll()
        {
            return _tourFileHandler.Load();
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

        public void Delete(Tour tour)
        {
            _tours = _tourFileHandler.Load();
            Tour founded = _tours.Find(a => a.Id == tour.Id);
            _tours.Remove(founded);
            _tourFileHandler.Save(_tours);
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

        bool MatchesFilters(Tour tour, string country, string city, int duration, GuideLanguage language, int numberOfGuests)
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
    }
}
