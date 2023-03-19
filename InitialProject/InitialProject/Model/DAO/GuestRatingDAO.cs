using InitialProject.FileHandler;
using InitialProject.Observer;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DAO
{
    public class GuestRatingDAO : ISubject
    {
        private readonly List<IObserver> _observers;
        private List<GuestRating> _guestRatings;
        private readonly Storage<GuestRating> _storage;
        private const string FilePath = "../../../Resources/Data/guestRatings";

        public GuestRatingDAO()
        {
            _storage = new Storage<GuestRating>(FilePath);
            _guestRatings = _storage.Load();
            _observers = new List<IObserver>();
        }
        public List<GuestRating> GetAll()
        {
            return _storage.Load();
        }

        public GuestRating Save(GuestRating guestRating)
        {
            _guestRatings = _storage.Load();
            _guestRatings.Add(guestRating);
            _storage.Save(_guestRatings);
            return guestRating;
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
