using InitialProject.Application.Observer;
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
        public Tour Update(Tour tour)
        {
            return _repository.Update(tour);
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

        public Tour GetById(int tourId)
        {
            return _repository.GetById(tourId);
        }

        public List<Tour> GetFiltered(string country, string city, int duration, GuideLanguage language, int NumberOfGuests)
        {
            return _repository.GetFiltered(country, city, duration, language, NumberOfGuests);
        }
        public List<Tour> SortByName(List<Tour> tours)
        {
            return _repository.SortByName(tours);
        }
        public List<Tour> SortByLocation(List<Tour> tours)
        {
            return _repository.SortByLocation(tours);
        }
        public List<Tour> SortByDuration(List<Tour> tours)
        {
            return _repository.SortByDuration(tours);
        }
        public List<Tour> SortByLanguage(List<Tour> tours)
        {
            return _repository.SortByLanguage(tours);
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
        public Tour GetMostVisited(String selectedYear)
        {
            return _repository.GetMostVisited(selectedYear);
        }
        public List<string> GetAvailableYears()
        {
            return _repository.GetAvailableYears();
        }
    }
}
