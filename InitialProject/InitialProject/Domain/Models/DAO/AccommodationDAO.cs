using InitialProject.Application.Observer;
using InitialProject.Application.Storage;
using InitialProject.Repositories.FileHandlers;
using InitialProject.Applications.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model.DAO
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
                if (MatchesFilters(accommodation, keyWords, type, guestNumber, numberOfDays))
                {
                    filteredAccommodations.Add(accommodation);
                }
            }
            return filteredAccommodations;
        }
        private bool MatchesFilters(Accommodation accommodation, string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            bool keyWordsMatch = Contains(keyWords, accommodation);
            bool typeMatch = accommodation.Type == type || type == AccommodationType.Everything;
            bool maximumGestsMatch = accommodation.MaximumGuests >= guestNumber;
            bool minimumDaysMatch = accommodation.MinimumDays <= numberOfDays;
            return keyWordsMatch && typeMatch && maximumGestsMatch && minimumDaysMatch;
        }
        private bool Contains(string keyWords, Accommodation accommodation)
        {
            string[] splitKeyWords = keyWords.Split(" ");
            foreach (string keyWord in splitKeyWords)
            {
                if (!(accommodation.Name.ToLower().Contains(keyWord.ToLower()) ||
                    accommodation.City.ToLower().Contains(keyWord.ToLower()) ||
                    accommodation.Country.ToLower().Contains(keyWord.ToLower())))
                {
                    return false;
                }
            }
            return true;
        }
        public List<Accommodation> SortByName(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.Name).ToList();
        }
        public List<Accommodation> SortByLocation(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.Country)
                                          .ThenBy(a => a.City)
                                          .ToList();
        }
        public List<Accommodation> SortByMaxGuestNumber(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MaximumGuests).ToList();
        }
        public List<Accommodation> SortByMinDaysNumber(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MinimumDays).ToList();
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
            return _accommodations.Count < 1 ? 1 : _accommodations.Max(a => a.Id) + 1;
        }

        public void Add(string name, string country, string city, string address, AccommodationType type, int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL,
                        User owner, int ownerId)
        {
            _accommodations = _storage.Load();
            int accommodationId = NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, address,
                type, maximumGuests, minimumDays, minimumCancelationNotice,
                                                            pictureURL, owner, ownerId);
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
