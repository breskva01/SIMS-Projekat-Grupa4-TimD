using InitialProject.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
using InitialProject.Storage;

namespace InitialProject.Model.DAO
{
    public class LocationDAO : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly List<Location> _locations;
        private readonly Storage<Location> _storage;
        private const string FilePath = "../../../Resources/Data/locations.csv";

        public LocationDAO()
        {
            _storage = new Storage<Location>(FilePath);
            _locations = _storage.Load();
            _observers = new List<IObserver>();
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
