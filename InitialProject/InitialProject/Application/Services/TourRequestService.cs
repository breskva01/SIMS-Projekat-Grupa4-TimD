using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<TourRequest> GetByUser(int userId)
        {
            return _repository.GetByUser(userId);
        }
        public List<TourRequest> GetApproved(List<TourRequest> tourRequests)
        {
            return _repository.GetApproved(tourRequests);
        }
        public int GetMonthNumberOfRequests(int month, string country, string city,int year, List<TourRequest> requests)
        {
            return _repository.GetMonthNumberOfRequests(month, country, city,year, requests);
        }
        public List<TourRequest> GetOnHold()
        {
            return _repository.GetOnHold();
        }
        public List<int> GetAllYears()
        {
            return _repository.GetAllYears();
        }
        public List<string> GetAllLocations(List<TourRequest> tourRequests)
        {
            return _repository.GetAllLocations(tourRequests);
        }
        public int GetLocationNumberOfRequests(string city, List<TourRequest> tourRequests)
        {
            return _repository.GetLocationNumberOfRequests(city, tourRequests);
        }

        public int GetYearNumberOfRequests(int year)
        {
            return _repository.GetYearNumberOfRequests(year);
        }
        public int GetYearNumberOfRequestsForChosenLocation(int year, string country, string city, List<TourRequest> requests)
        {
            return _repository.GetYearNumberOfRequestsForChosenLocation(year, country, city, requests);
        }
        public int GetLanguageNumberOfRequests(GuideLanguage guideLanguage)
        {
            return _repository.GetLanguageNumberOfRequests(guideLanguage);
        }
        public int GetApprovedForYear(string year)
        {
            return _repository.GetApprovedForYear(year);
        }
        public int GetApprovedGeneral()
        {
            return _repository.GetApprovedGeneral();
        }
        public List<string> GetAvailableYears()
        {
            return _repository.GetAvailableYears();
        }
        public int GetGeneralNumberOfGuests()
        {
            return _repository.GetGeneralNumberOfGuests();
        }
        public List<TourRequest> GetForChosenLocation(List<TourRequest> requests, string country, string city)
        {
            return _repository.GetForChosenLocation(requests,country, city);
        }
        public List<int> GetRangeOfYears(List<TourRequest> requests) 
        {
            return _repository.GetRangeOfYears(requests);
        }
        public int GetYearNumberOfGuests(string year)
        {
            return _repository.GetYearNumberOfGuests(year);
        }
        public TourRequest CreateTourRequest(Location Location, string Description,RequestStatus Status, GuideLanguage Language,
            int NumberOfGuests, DateTime EarliestDate, DateTime LatestDate, int TourId)
        {
            TourRequest TourRequest = new TourRequest();
            TourRequest.Location = Location;
            TourRequest.Description = Description;
            TourRequest.Status = Status;
            TourRequest.Language = Language;
            TourRequest.NumberOfGuests = NumberOfGuests;
            TourRequest.EarliestDate = EarliestDate;
            TourRequest.LatestDate = LatestDate;
            TourRequest.TourId = TourId;

            return _repository.Save(TourRequest);
        }
        public List<TourRequest> GetFiltered(string country, string city, DateTime date1, DateTime date2, int number, string language)
        {
            return _repository.GetFiltered(country, city, date1, date2, number, language);
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
