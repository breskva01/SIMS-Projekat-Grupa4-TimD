using InitialProject.FileHandler;
using InitialProject.Observer;
using InitialProject.Serializer;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DAO
{
    public class AccommodationDAO : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly Storage<Accommodation> _storage;
        private List<Accommodation> _accommodations;
        private const string FilePath = "../../../Resources/Data/accommodations.csv";


        public AccommodationDAO()
        {
            _storage = new Storage<Accommodation>(FilePath);
            _accommodations = _storage.Load();
            _observers = new List<IObserver>();
        }
        public List<Accommodation> GetAll()
        {
            return _storage.Load();
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            _accommodations = _storage.Load();
            List<Accommodation> filteredAccommodations = new();

            foreach (Accommodation accommodation in _accommodations)
            {
                bool keyWordsMatch = CheckIfContainted(keyWords, accommodation);
                bool typeMatch = accommodation.Type == type || type == AccommodationType.Everything;
                bool maximumGestsMatch = accommodation.MaximumGuests >= guestNumber;
                bool minimumDaysMatch = (accommodation.MinimumDays <= numberOfDays) || numberOfDays == 0;
                if (keyWordsMatch && typeMatch && maximumGestsMatch && minimumDaysMatch)
                    {
                        filteredAccommodations.Add(accommodation);
                    }   
            }
            return filteredAccommodations;
        }
        public bool CheckIfContainted(string keyWords, Accommodation accommodation)
        {
            if (accommodation.Name.ToLower().Contains(keyWords.ToLower()) || 
                accommodation.City.ToLower().Contains(keyWords.ToLower()) ||
                accommodation.Country.ToLower().Contains(keyWords.ToLower()))
            {
                return true;
            }
            return false;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _storage.Load();
            _accommodations.Add(accommodation);
            _storage.Save(_accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _storage.Load();
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(a => a.Id) + 1;
        }

        public void Add(string name, string country, string city, AccommodationType type, int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL)
        {
            _accommodations = _storage.Load();
            int accommodationId = NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURL);
            _accommodations.Add(accommodation);
            _storage.Save(_accommodations);
            NotifyObservers();
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _storage.Load();
            Accommodation founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _storage.Save(_accommodations);
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
