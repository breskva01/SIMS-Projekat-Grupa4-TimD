﻿using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class LocationRepository : ILocationRepository
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
        public Location GetById(int id)
        {
            return _locations.Find(x => x.Id == id);
        }

    }
}
