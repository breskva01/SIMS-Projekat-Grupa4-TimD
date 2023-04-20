using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationRatingService : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationRatingRepository _repository;
        public AccommodationRatingService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<IAccommodationRatingRepository>();
        }
        public List<AccommodationRating> GetAll()
        {
            return _repository.GetAll();
        }
        public List<AccommodationRating> GetByOwnerId(int ownerId)
        {
            return _repository.GetByOwnerId(ownerId);
        }
        public List<AccommodationRating> GetEligibleForDisplay(int ownerId)
        {
            return _repository.GetEligibleForDisplay(ownerId);
        }
        public void Save(AccommodationReservation reservation, int location, int hygiene, int pleasantness,
                         int fairness, int parking, string comment, List<string> pictureURLs)
        {
            var rating = new AccommodationRating(reservation, location, hygiene, pleasantness, fairness,
                                                 parking, comment, pictureURLs);
            _repository.Save(rating);
            RepositoryInjector.Get<IAccommodationReservationRepository>().MarkOwnerAsRated(reservation.Id);
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
            _observers.ForEach(o => o.Update());
        }
    }
}
