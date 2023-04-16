﻿using InitialProject.Application.Observer;
using InitialProject.Application.Storage;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationService : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationRepository _repository;
        public AccommodationService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryStore.GetIAccommodationRepository;
        }
        public List<Accommodation> GetAll()
        {
            return _repository.GetAll();
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            return _repository.GetFiltered(keyWords, type, guestNumber, numberOfDays);
        }
        public List<Accommodation> Sort(List<Accommodation> accommodations, string criterium)
        {
            return _repository.Sort(accommodations, criterium);
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
