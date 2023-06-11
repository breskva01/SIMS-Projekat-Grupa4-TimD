using InitialProject.Application.Injector;
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
    public class ComplexTourRequestService
    {
        private readonly List<IObserver> _observers;
        private readonly IComplexTourRequestRepository _repository;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;

        public ComplexTourRequestService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<IComplexTourRequestRepository>();
            _tourRequestService = new TourRequestService();
            _tourService = new TourService();
        }

        public List<ComplexTourRequest> GetAll()
        {
            List<ComplexTourRequest> complexTourRequests = _repository.GetAll();
            foreach(ComplexTourRequest complexTourRequest in complexTourRequests)
            {
                FillTourRequestList(complexTourRequest);
            }
            return complexTourRequests;
        }

        public void FillTourRequestList(ComplexTourRequest complexTourRequest)
        {
            foreach(int id in complexTourRequest.TourRequestIDs) 
            {
                complexTourRequest.TourRequests.Add(_tourRequestService.GetById(id));
            }
        }

        public ComplexTourRequest GetById(int complexTourRequestId)
        {
            return _repository.GetById(complexTourRequestId);
        }
        public List<ComplexTourRequest> GetByUser(int userId)
        {
            List<ComplexTourRequest> userRequests = _repository.GetByUser(userId, GetAll());
            userRequests = CreateRequestPartsForAll(userRequests);
            return userRequests;
        }


        public List<ComplexTourRequest> CreateRequestPartsForAll(List<ComplexTourRequest> complexTourRequests)
        {
            foreach (ComplexTourRequest complexTourRequest in complexTourRequests)
            {
                CreateRequestPartsForOne(complexTourRequest);
            }

            return complexTourRequests;
        }

        public void CreateRequestPartsForOne(ComplexTourRequest complexTourRequest)
        {
            int i = 1;
            foreach(TourRequest tourRequest in complexTourRequest.TourRequests)
            {
                string requestPart = "Part " + i.ToString() + ": " + tourRequest.Status.ToString();
                if(tourRequest.Status == RequestStatus.Approved)
                {
                    Tour tour = _tourService.GetById(tourRequest.TourId);
                    requestPart += " | Date: " + tour.Start.ToString("dd-MM-yyyy");
                }
                complexTourRequest.RequestParts.Add(requestPart);
                i++;
            }
        }

        public List<ComplexTourRequest> GetApproved(List<ComplexTourRequest> complexTourRequests)
        {
            return _repository.GetApproved(complexTourRequests);
        }
        public List<ComplexTourRequest> GetOnHold()
        {
            return _repository.GetOnHold();
        }
        
        public ComplexTourRequest CreateComplexTourRequest(int userId, ComplexRequestStatus Status, List<TourRequest> tourRequests)
        {
            ComplexTourRequest ComplexTourRequest = new ComplexTourRequest();
            ComplexTourRequest.UserId = userId;
            ComplexTourRequest.Status = Status;
            ComplexTourRequest.TourRequests = tourRequests;
            foreach(TourRequest t in tourRequests)
            {
                ComplexTourRequest.TourRequestIDs.Add(t.Id);
            }

            return _repository.Save(ComplexTourRequest);
        }
        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest)
        {
            return _repository.Update(complexTourRequest);
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
