using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class LocationFileHandler
    {
        private const string _locationsFilePath = "../../../Resources/Data/locations.csv";
        private readonly Serializer<Location> _serializer;

        public LocationFileHandler()
        {
            _serializer = new Serializer<Location>();
        }
        public List<Location> Load()
        {
            return _serializer.FromCSV(_locationsFilePath);
        }
    }
}
