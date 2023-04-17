using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class LocationService
    {
        private readonly List<IObserver> _observers;
        private readonly LocationRepository _repository;
        public LocationService()
        {
            _observers = new List<IObserver>();
            _repository = new LocationRepository();
        }
        public List<Location> GetAll()
        {
            return _repository.GetAll();
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
