using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Observer;
using InitialProject.Serializer;
using InitialProject.Storage;

namespace InitialProject.Model.DAO
{
    public class TourDAO : ISubject
    {
        private List<Tour> _tours;
        private readonly List<IObserver> _observers;
        private readonly Storage<Tour> _storage;
        private const string FilePath = "../../../Resources/Data/tours.csv";

        public TourDAO()
        {
            _storage = new Storage<Tour>(FilePath);
            _tours = _storage.Load();
            _observers = new List<IObserver>(); 
        }
        public List<Tour> GetAll()
        {
            return _storage.Load();
        }
        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _storage.Load();
            _tours.Add(tour);
            _storage.Save(_tours);
            return tour;
        }
        public Tour Update(Tour tour)
        {
            _tours = _storage.Load();
            Tour updated = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(updated);
            _tours.Add(tour);
            _storage.Save(_tours);
            return tour;
        }
        public int NextId()
        {
            _tours = _storage.Load();
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }
        
        public void Delete(Tour tour)
        {
            _tours = _storage.Load();
            Tour founded = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(founded);
            _storage.Save(_tours);
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
