using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Repositories
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private readonly TourRequestFileHandler _tourRequestFileHandler;
        private readonly LocationFileHandler _locationFileHandler;
        private readonly UserFileHandler _userFileHandler;
        private List<TourRequest> _tourRequests;
        private List<Location> _locations;

        public TourRequestRepository()
        {
            _tourRequestFileHandler = new TourRequestFileHandler();
            _locationFileHandler = new LocationFileHandler();
            _userFileHandler = new UserFileHandler();
            _tourRequests = _tourRequestFileHandler.Load();
        }

        public void Delete(TourRequest tourRequest)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            TourRequest founded = _tourRequests.Find(a => a.Id == tourRequest.Id);
            _tourRequests.Remove(founded);
            _tourRequestFileHandler.Save(_tourRequests);
        }

        public List<TourRequest> GetAll()
        {
            return _tourRequestFileHandler.Load();
        }

        public List<TourRequest> GetApproved(List<TourRequest> userRequests)
        { 
            List<TourRequest> ApprovedRequests = new List<TourRequest>();
            foreach (TourRequest t in userRequests)
            {
                if (t.Status == RequestStatus.Approved)
                {
                    ApprovedRequests.Add(t);
                }
            }
            return ApprovedRequests;
        }
        public int GetApprovedForYear(string year)
        {
            List<TourRequest> allRequests = GetAll();
            double approvedCounter = 0;
            foreach (TourRequest t in allRequests)
            {
                if (t.EarliestDate.Year.ToString().Equals(year) && t.Status == RequestStatus.Approved)
                {
                    approvedCounter++;
                }
            }
            double result = approvedCounter / allRequests.Count * 100;
            int roundedResult = (int)Math.Round(result);
            return roundedResult;
        }

        public int GetApprovedGeneral()
        {
            List<TourRequest> allRequests = GetAll();
            double approvedCounter = 0;
            foreach (TourRequest t in allRequests)
            {
                if (t.Status == RequestStatus.Approved)
                {
                    approvedCounter++;
                }
            }
            double result = approvedCounter / allRequests.Count * 100;
            int roundedResult = (int)Math.Round(result);
            return roundedResult;
        }
        public List<string> GetAvailableYears()
        {
            List<string> years = new List<string>();
            List<TourRequest> tourRequests = GetAll();
            foreach (TourRequest t in tourRequests)
            {
                if (!years.Contains(t.EarliestDate.Year.ToString()))
                {
                    years.Add(t.EarliestDate.Year.ToString());
                }
            }
            return years;
        }

        public int GetGeneralNumberOfGuests()
        {

            double totalGuests = 0;
            int numberOfApprovedRequests = 0; 
            List<TourRequest> ApprovedRequests = GetApproved(GetAll());
            foreach (TourRequest t in ApprovedRequests)
            {
                totalGuests += t.NumberOfGuests;
                numberOfApprovedRequests++;
            }

            if (numberOfApprovedRequests == 0)
            {
                return 0;
            }

            return (int)Math.Round(totalGuests / numberOfApprovedRequests);
        }

        public int GetYearNumberOfGuests(string year)
        {

            double totalGuests = 0;
            int numberOfApprovedRequests = 0;
            List<TourRequest> ApprovedRequests = GetApproved(GetAll());
            foreach (TourRequest t in ApprovedRequests)
            {
                if (t.EarliestDate.Year.ToString().Equals(year))
                {
                    totalGuests += t.NumberOfGuests;
                    numberOfApprovedRequests++;
                }
            }

            if (numberOfApprovedRequests == 0)
            {
                return 0;
            }

            return (int)Math.Round(totalGuests / numberOfApprovedRequests);
        }



        public List<TourRequest> GetOnHold()
        {
            List<TourRequest> OnHoldRequests = new List<TourRequest>();
            foreach (TourRequest t in GetAll())
            {
                if (t.Status == RequestStatus.OnHold)
                {
                    OnHoldRequests.Add(t);
                }
            }
            return OnHoldRequests;
        }

        public TourRequest GetById(int tourRequestId)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            return _tourRequests.Find(v => v.Id == tourRequestId);
        }

        public List<TourRequest> GetByUser(int userId)
        {
            List<TourRequest> tourRequests = GetAll();
            foreach (TourRequest t in tourRequests)
            {
                if (t.UserId == userId)
                {
                    tourRequests.Add(t);
                }
            }

            tourRequests = CheckIfInvalid(tourRequests);
            return tourRequests;
        }

        public List<TourRequest> CheckIfInvalid(List<TourRequest> tourRequests)
        {
            TimeSpan timeDifference;
            foreach (TourRequest t in tourRequests)
            {
                timeDifference = t.EarliestDate - DateTime.Now;
                if(timeDifference.TotalHours < 48)
                {
                    t.Status = RequestStatus.Invalid;
                }
            }
            return tourRequests;
        }

        public int NextId()
        {
            _tourRequests = _tourRequestFileHandler.Load();
            if (_tourRequests.Count < 1)
            {
                return 1;
            }
            return _tourRequests.Max(t => t.Id) + 1;
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _tourRequests = _tourRequestFileHandler.Load();
            _tourRequests.Add(tourRequest);
            _tourRequestFileHandler.Save(_tourRequests);
            return tourRequest;
        }

        public TourRequest Update(TourRequest tourRequest)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            TourRequest updated = _tourRequests.Find(t => t.Id == tourRequest.Id);
            _tourRequests.Remove(tourRequest);
            _tourRequests.Add(tourRequest);
            _tourRequestFileHandler.Save(_tourRequests);
            return tourRequest;
        }
    }
}
