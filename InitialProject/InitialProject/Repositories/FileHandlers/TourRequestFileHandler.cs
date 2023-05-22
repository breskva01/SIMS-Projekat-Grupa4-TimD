using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class TourRequestFileHandler
    {
        private const string _tourRequestsFilePath = "../../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializer;
        public TourRequestFileHandler()
        {
            _serializer = new Serializer<TourRequest>();
        }
        public List<TourRequest> Load()
        {
            return _serializer.FromCSV(_tourRequestsFilePath);
        }
        public void Save(List<TourRequest> tourRequests)
        {
            _serializer.ToCSV(_tourRequestsFilePath, tourRequests);
        }
    }
}
