﻿using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Domain.Models.DAO;
using InitialProject.Repositories;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class TourService
    {
        private readonly List<IObserver> _observers;
        private readonly TourRepository _repository;
        public TourService()
        {
            _observers = new List<IObserver>();
            _repository = new TourRepository();
        }
        public List<Tour> GetAll()
        {
            return _repository.GetAll();
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public Tour CreateTour(string Name, Location Location, string Description, GuideLanguage Language,
            int MaximumGuests, DateTime Start, int Duration, string PictureUrl, List<KeyPoint> ky, List<int> kyIds)
        {
            int LocationId = Location.Id;
            Tour Tour = new Tour();
            Tour.Name = Name;
            Tour.Location = Location;
            Tour.LocationId = LocationId;
            Tour.Description = Description;
            Tour.Language = Language;
            Tour.MaximumGuests = MaximumGuests;
            Tour.Start = Start;
            Tour.Duration = Duration;
            Tour.PictureURL = PictureUrl;
            Tour.CurrentNumberOfGuests = 0;
            Tour.KeyPoints = ky;
            Tour.KeyPointIds = kyIds;
            return _repository.Save(Tour);
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
