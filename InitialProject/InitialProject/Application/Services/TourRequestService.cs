﻿using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class TourRequestService
    {
        private readonly List<IObserver> _observers;
        private readonly ITourRequestRepository _repository;
        public TourRequestService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<ITourRequestRepository>();
        }

        public List<TourRequest> GetAll()
        {
            return _repository.GetAll();
        }
        public TourRequest GetById(int tourRequestId)
        {
            return _repository.GetById(tourRequestId);
        }
        public TourRequest CreateTourRequest(Location Location, string Description, GuideLanguage Language,
            int NumberOfGuests, DateTime EarliestDate, DateTime LatestDate)
        {
            TourRequest TourRequest = new TourRequest();
            TourRequest.Location = Location;
            TourRequest.Description = Description;
            TourRequest.Language = Language;
            TourRequest.NumberOfGuests = NumberOfGuests;
            TourRequest.EarliestDate = EarliestDate;
            TourRequest.LatestDate = LatestDate;

            return _repository.Save(TourRequest);
        }
        public TourRequest Update(TourRequest tourRequest)
        {
            return _repository.Update(tourRequest);
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
