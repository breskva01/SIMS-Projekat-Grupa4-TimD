using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class ComplexTourRequestRepository : IComplexTourRequestRepository
    {
        private readonly ComplexTourRequestFileHandler _complexTourRequestFileHandler;
        private readonly UserFileHandler _userFileHandler;
        private List<ComplexTourRequest> _complexTourRequests;

        public ComplexTourRequestRepository()
        {
            _complexTourRequestFileHandler = new ComplexTourRequestFileHandler();
            _userFileHandler = new UserFileHandler();
            _complexTourRequests = _complexTourRequestFileHandler.Load();
        }

        public void Delete(ComplexTourRequest complexTourRequest)
        {
            _complexTourRequests = _complexTourRequestFileHandler.Load();
            ComplexTourRequest founded = _complexTourRequests.Find(a => a.Id == complexTourRequest.Id);
            _complexTourRequests.Remove(founded);
            _complexTourRequestFileHandler.Save(_complexTourRequests);
        }

        public List<ComplexTourRequest> GetAll()
        {
            return _complexTourRequestFileHandler.Load();
        }

        public List<ComplexTourRequest> GetApproved(List<ComplexTourRequest> userRequests)
        {
            List<ComplexTourRequest> ApprovedRequests = new List<ComplexTourRequest>();
            foreach (ComplexTourRequest t in userRequests)
            {
                if (t.Status == ComplexRequestStatus.Approved)
                {
                    ApprovedRequests.Add(t);
                }
            }
            return ApprovedRequests;
        }
        
        public List<ComplexTourRequest> GetOnHold()
        {
            List<ComplexTourRequest> OnHoldRequests = new List<ComplexTourRequest>();
            foreach (ComplexTourRequest t in GetAll())
            {
                if (t.Status == ComplexRequestStatus.OnHold)
                {
                    OnHoldRequests.Add(t);
                }
            }
            return OnHoldRequests;
        }

        public ComplexTourRequest GetById(int complexTourRequestId)
        {
            _complexTourRequests = _complexTourRequestFileHandler.Load();
            return _complexTourRequests.Find(v => v.Id == complexTourRequestId);
        }
        

        public List<ComplexTourRequest> GetByUser(int userId)
        {
            List<ComplexTourRequest> complexTourRequests = GetAll();
            foreach (ComplexTourRequest t in complexTourRequests)
            {
                if (t.UserId == userId)
                {
                    complexTourRequests.Add(t);
                }
            }

            complexTourRequests = CheckIfInvalid(complexTourRequests);
            return complexTourRequests;
        }

        public List<ComplexTourRequest> CheckIfInvalid(List<ComplexTourRequest> complexTourRequests)
        {
            TimeSpan timeDifference;
            foreach (ComplexTourRequest t in complexTourRequests)
            {
                timeDifference = GetEarliestDate(t) - DateTime.Now;
                if (timeDifference.TotalHours < 48)
                {
                    t.Status = ComplexRequestStatus.Invalid;
                }
            }
            return complexTourRequests;
        }

        public DateTime GetEarliestDate(ComplexTourRequest complexTourRequest)
        {
            TourRequest firstPartOfComplexRequest = complexTourRequest.TourRequests[0];
            return firstPartOfComplexRequest.EarliestDate;
        }
        

       
        public int NextId()
        {
            _complexTourRequests = _complexTourRequestFileHandler.Load();
            if (_complexTourRequests.Count < 1)
            {
                return 1;
            }
            return _complexTourRequests.Max(t => t.Id) + 1;
        }

        public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
        {
            complexTourRequest.Id = NextId();
            _complexTourRequests = _complexTourRequestFileHandler.Load();
            _complexTourRequests.Add(complexTourRequest);
            _complexTourRequestFileHandler.Save(_complexTourRequests);
            return complexTourRequest;
        }

        public ComplexTourRequest Update(ComplexTourRequest complexTourRequest)
        {
            _complexTourRequests = _complexTourRequestFileHandler.Load();
            ComplexTourRequest updated = _complexTourRequests.Find(t => t.Id == complexTourRequest.Id);
            _complexTourRequests.Remove(updated);
            _complexTourRequests.Add(complexTourRequest);
            _complexTourRequestFileHandler.Save(_complexTourRequests);
            return complexTourRequest;
        }
    }
}
