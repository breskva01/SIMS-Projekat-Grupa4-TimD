using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class TourRepository
    {
        private readonly TourFileHandler _fileHandler;
        private List<Tour> _tours;

        public TourRepository()
        {
            _fileHandler = new TourFileHandler();
            _tours = _fileHandler.Load();
        }

        public List<Tour> GetAll()
        {
            return _fileHandler.Load();
        }

        public Tour Update(Tour tour)
        {
            _tours = _fileHandler.Load();
            Tour updated = _tours.Find(t => t.Id == tour.Id);
            _tours.Remove(updated);
            _tours.Add(tour);
            _fileHandler.Save(_tours);
            return tour;
        }
        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _fileHandler.Load();
            _tours.Add(tour);
            _fileHandler.Save(_tours);
            return tour;
        }

        public int NextId()
        {
            _tours = _fileHandler.Load();
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _fileHandler.Load();
            Tour founded = _tours.Find(a => a.Id == tour.Id);
            _tours.Remove(founded);
            _fileHandler.Save(_tours);
        }
    }
}
