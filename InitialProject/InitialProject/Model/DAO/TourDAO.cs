using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InitialProject.Observer;
using InitialProject.Serializer;
using InitialProject.Storage;

namespace InitialProject.Model.DAO
{
    public class TourDAO : ISubject
    {
        private List<Tour> _tours;
        private List<Location> _locations;
        private readonly List<IObserver> _observers;
        private readonly Storage<Tour> _tourStorage;
        private readonly Storage<Location> _locationStorage;
        private const string TourFilePath = "../../../Resources/Data/tours.csv";
        private const string LocationFilePath = "../../../Resources/Data/locations.csv";

        public TourDAO() 
        {
            _tourStorage = new Storage<Tour>(TourFilePath);
            _locationStorage = new Storage<Location>(LocationFilePath);
            _tours = _tourStorage.Load();
            //_cities = _cityStorage.Load();
            _observers = new List<IObserver>(); 
        }
        public List<Tour> GetAll()
        {
            return _tourStorage.Load();
        }
        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _tourStorage.Load();
            _tours.Add(tour);
            _tourStorage.Save(_tours);
            return tour;
        }
        public Tour Update(Tour tour)
        {
            _tours = _tourStorage.Load();
            Tour updated = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(updated);
            _tours.Add(tour);
            _tourStorage.Save(_tours);
            return tour;
        }
        public int NextId()
        {
            _tours = _tourStorage.Load();
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }
        
        public void Delete(Tour tour)
        {
            _tours = _tourStorage.Load();
            Tour founded = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(founded);
            _tourStorage.Save(_tours);
        }

        public List<Tour> GetFiltered(string country, string city, int duration, GuideLanguage language, int numberOfGuests)
        {
            _tours = _tourStorage.Load();
            _locations = _locationStorage.Load();
            foreach (Tour t in _tours)
            {
                t.Location = _locations.FirstOrDefault(l => l.Id == t.LocationId);
            }

            List<Tour> filteredTours = new();
            foreach (Tour tour in _tours)
            {
                bool countryMatch = tour.Location.Country == country || country == "";
                bool cityMatch = tour.Location.City == city || city =="";
                bool durationMatch = tour.Duration == duration || duration == 0;
                bool languageMatch = tour.Language == language || language == GuideLanguage.All;
                bool numberOfGuestsMatch = (tour.MaximumGuests - tour.CurrentNumberOfGuests) >= numberOfGuests ||  numberOfGuests == 0;
                if (countryMatch && cityMatch && durationMatch && languageMatch && numberOfGuestsMatch)
                {
                    filteredTours.Add(tour);
                }
            }
            return filteredTours;
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
