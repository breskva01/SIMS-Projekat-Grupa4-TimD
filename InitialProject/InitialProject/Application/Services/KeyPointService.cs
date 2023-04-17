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
    public class KeyPointService
    {
        private readonly List<IObserver> _observers;
        private readonly KeyPointRepository _repository;
        public KeyPointService()
        {
            _observers = new List<IObserver>();
            _repository = new KeyPointRepository();
        }
        public List<KeyPoint> GetAll()
        {
            return _repository.GetAll();
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        /*
        public KeyPoint Update(KeyPoint keyPoint)
        {
            return _repository.Update(keyPoint);
        }
        */
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
