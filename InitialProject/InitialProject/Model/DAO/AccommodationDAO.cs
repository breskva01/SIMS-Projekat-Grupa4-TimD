using InitialProject.Observer;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DAO
{
    class AccommodationDAO : ISubject
    {

        private readonly List<IObserver> _observers;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly List<Accommodation> _accommodations;

        public AccommodationDAO() 
        { 
            _accommodationRepository= new AccommodationRepository();
            _accommodations = _accommodationRepository.GetAll();
            _observers= new List<IObserver>();
        }

        public Accommodation GetId(int accommodationId) 
        {
            return _accommodations.Find(a => a.Id == accommodationId);
        }

        public void Add(string name, string country, string city, AccommodationType type , int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL)
        {
            int accommodationId = _accommodationRepository.NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURL);
            _accommodationRepository.Save(accommodation);
            NotifyObservers();
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
