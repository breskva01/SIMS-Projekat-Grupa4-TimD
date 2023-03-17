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
        private readonly AccommodationFileHandler _fileHandler;
        private List<Accommodation> _accommodations;
        public AccommodationDAO()
        {
            _fileHandler = new AccommodationFileHandler();
            _accommodations = _fileHandler.Load();
            _observers = new List<IObserver>();
        }
        public List<Accommodation> GetAll()
        {
            return _fileHandler.Load();
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            _accommodations = _fileHandler.Load();
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
            _accommodations = _fileHandler.Load();
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _fileHandler.Load();
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(a => a.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _fileHandler.Load();
            Accommodation founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _fileHandler.Save(_accommodations);
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
