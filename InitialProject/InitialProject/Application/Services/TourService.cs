﻿using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repository;
using InitialProject.WPF.ViewModels;
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
        private readonly ITourRepository _repository;

        public TourService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<ITourRepository>();
        }

        public List<Tour> GetAll()
        {
            return _repository.GetAll();
        }
        public Tour GetById(int tourId)
        {
            return _repository.GetById(tourId);
        }
        public Tour GetByName(String name)
        {
            return _repository.GetByName(name);
        }
        public Tour CreateTour(string Name, Location Location, string Description, GuideLanguage Language,
            int MaximumGuests, DateTime Start, int Duration, string PictureUrl, List<KeyPoint> ky, List<int> kyIds, int userId)
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
            Tour.GuideId = userId;

            return _repository.Save(Tour);
        }
        public Tour Update(Tour tour)
        {
            return _repository.Update(tour);
        }
        
        public bool IsActive(int tourId)
        {
            return _repository.IsActive(tourId);
        }
        public List<Tour> GetFiltered(TourFilterSort tourFilterSort)
        {
            return _repository.GetFiltered(tourFilterSort);
        }

        public List<Tour> GetSorted(List<Tour> tours, TourFilterSort tourFilterSort)
        {
            return _repository.GetSorted(tours, tourFilterSort);
        }
        public List<Tour> SortByCountry(List<Tour> tours)
        {
            return _repository.SortByCountry(tours);
        }
        public List<Tour> SortByCity(List<Tour> tours)
        {
            return _repository.SortByCity(tours);
        }
        public List<Tour> SortByDuration(List<Tour> tours)
        {
            return _repository.SortByDuration(tours);
        }
        public List<Tour> SortByLanguage(List<Tour> tours)
        {
            return _repository.SortByLanguage(tours);
        }
        public List<Tour> SortBySpaces(List<Tour> tours)
        {
            return _repository.SortBySpaces(tours);
        }

        public Tour GetMostVisited(String selectedYear)
        {
            return _repository.GetMostVisited(selectedYear);
        }
        public List<string> GetAvailableYears()
        {
            return _repository.GetAvailableYears();
        }
        public List<string> GetFinishedTourNames()
        {
            return _repository.GetFinishedTourNames();
        }

        public string GetNumberOfGuestBelow18(Tour tour)
        {
            return _repository.GetNumberOfGuestBelow18(tour);
        }
        public string GetNumberOfMiddleAgeGuests(Tour tour)
        {
            return _repository.GetNumberOfMiddleAgeGuests(tour);
        }
        public string GetNumberOfOlderGuests(Tour tour)
        {
            return _repository.GetNumberOfOlderGuests(tour);
        }
        public List<Tour> GetFinishedTours()
        {
            return _repository.GetFinishedTours();
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
        public string CheckSuperGuide(int GuideId)
        {
            return _repository.CheckSuperGuide(GuideId);
        }

    }
}
