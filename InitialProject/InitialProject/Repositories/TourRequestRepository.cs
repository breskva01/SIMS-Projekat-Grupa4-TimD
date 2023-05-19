using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public TourRequest GetById(int tourRequestId)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            return _tourRequests.Find(v => v.Id == tourRequestId);
        }
        public List<TourRequest> GetFiltered(string country, string city, DateTime date1, DateTime date2, int numberOfGuests, string language)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            _locations = _locationFileHandler.Load();

            foreach (TourRequest t in _tourRequests)
            {
                t.Location = _locations.FirstOrDefault(l => l.Id == t.Location.Id);
            }
            List<TourRequest> filteredTourRequests = new();
            foreach (TourRequest request in _tourRequests)
            {
                if (MatchesFilters(request, country, city, date1, date2, numberOfGuests, language))
                {
                    filteredTourRequests.Add(request);
                }
            }
            return filteredTourRequests;
        }
        public bool MatchesFilters(TourRequest tourRequest, string country, string city, DateTime earliestDate, DateTime latestDate, int numberOfGuests, string language)
        {
            bool countryMatch = tourRequest.Location.Country == country || country == null;
            bool cityMatch = tourRequest.Location.City == city || city == null;
            bool dateMatch = (tourRequest.EarliestDate > earliestDate && tourRequest.LatestDate < latestDate) || earliestDate == Convert.ToDateTime("01/01/0001 00:00:00") && latestDate == Convert.ToDateTime("01/01/0001 00:00:00");
            bool numberMatch = tourRequest.NumberOfGuests == numberOfGuests || numberOfGuests == 0;
            bool languageMatch = tourRequest.Language.ToString() == language || language == null;

            return countryMatch && cityMatch && dateMatch && numberMatch && languageMatch;
            /*
            bool countryMatch = tour.Location.Country == country || country == "";
            bool cityMatch = tour.Location.City == city || city == "";
            bool durationMatch = tour.Duration == duration || duration == 0;
            bool languageMatch = tour.Language == language || language == GuideLanguage.All;
            bool numberOfGuestsMatch = tour.MaximumGuests - tour.CurrentNumberOfGuests >= numberOfGuests || numberOfGuests == 0;
            */
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
            _tourRequests.Remove(updated);
            _tourRequests.Add(tourRequest);
            _tourRequestFileHandler.Save(_tourRequests);
            return tourRequest;
        }
    }
}
