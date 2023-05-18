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

        public TourRequest GetById(int tourRequestId)
        {
            _tourRequests = _tourRequestFileHandler.Load();
            return _tourRequests.Find(v => v.Id == tourRequestId);
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
