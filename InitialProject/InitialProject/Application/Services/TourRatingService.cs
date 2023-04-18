using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class TourRatingService
    {
        private readonly List<IObserver> _observers;
        //private readonly ITourRatingRepository _repository;
        private readonly TourRatingRepository _repository;
        public TourRatingService()
        {
            _observers = new List<IObserver>();
            //_repository = RepositoryStore.GetIAccommodationRatingRepository;
            _repository = new TourRatingRepository();
        }
        public List<TourRating> GetAll()
        {
            return _repository.GetAll();
        }

        public TourRating Get(int id)
        {
            return _repository.Get(id);
        }

        public TourRating Save(int guideKnowledge, int guideLanguage, int tourInteresting,
                         int tourInformative, int tourContent, string comment, List<string> pictureURLs)
        {
            var rating = new TourRating()
            {
                GuideKnowledge = guideKnowledge,
                GuideLanguage = guideLanguage,
                TourInteresting = tourInteresting,
                TourInformative = tourInformative,
                TourContent = tourContent,
                Comment = comment,
                PictureURLs = pictureURLs
            };
            return _repository.Save(rating);
            //RepositoryStore.GetIAccommodationReservationRepository.MarkOwnerAsRated(reservation.Id);
        }

        public TourRating Update(TourRating rating)
        {
            return _repository.Update(rating);
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
