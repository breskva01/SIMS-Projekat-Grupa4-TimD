using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class ComplexTourRequestFileHandler
    {
        private const string _complexTourRequestsFilePath = "../../../Resources/Data/complexTourRequests.csv";
        private readonly Serializer<ComplexTourRequest> _serializer;
        public ComplexTourRequestFileHandler()
        {
            _serializer = new Serializer<ComplexTourRequest>();
        }
        public List<ComplexTourRequest> Load()
        {
            return _serializer.FromCSV(_complexTourRequestsFilePath);
        }
        public void Save(List<ComplexTourRequest> complexTourRequests)
        {
            _serializer.ToCSV(_complexTourRequestsFilePath, complexTourRequests);
        }
    }
}
