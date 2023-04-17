using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class LocationRepository
    {
        private readonly LocationFileHandler _fileHandler;
        private List<Location> _locations;

        public LocationRepository()
        {
            _fileHandler = new LocationFileHandler();
            _locations = _fileHandler.Load();
        }

        public List<Location> GetAll()
        {
            return _fileHandler.Load();
        }
        public Location Get(int id)
        {
            return _locations.Find(x => x.Id == id);
        }

    }
}
