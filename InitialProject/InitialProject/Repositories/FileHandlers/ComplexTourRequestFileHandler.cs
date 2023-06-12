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
            var complexTourRequests = _serializer.FromCSV(_complexTourRequestsFilePath);
            FillTourRequests(complexTourRequests);
            FillUsers(complexTourRequests);
            return complexTourRequests;
        }
        private void FillTourRequests(List<ComplexTourRequest> complexTourRequests)
        {
            var requests = new TourRequestFileHandler().Load();

            foreach (var complexTourRequest in complexTourRequests)
            {
                complexTourRequest.TourRequests = new List<TourRequest>();
                foreach(int id in complexTourRequest.TourRequestIDs)
                {
                    foreach(TourRequest request in requests)
                    {
                        if(id == request.Id)
                        {
                            complexTourRequest.TourRequests.Add(request);
                            
                        }
                    }
                }
                FillLocations(complexTourRequest.TourRequests);
            }
        }
        private void FillUsers(List<ComplexTourRequest> complexTourRequests)
        {
            var users = new UserFileHandler().Load();
            complexTourRequests.ForEach(r =>
                r.User = users.Find(user => user.Id == r.UserId));
        }
        private void FillLocations(List<TourRequest> tourRequests)
        {
            var locations = new LocationFileHandler().Load();
            tourRequests.ForEach(r =>
                r.Location = locations.Find(loc => loc.Id == r.Location.Id));
        }
        public void Save(List<ComplexTourRequest> complexTourRequests)
        {
            _serializer.ToCSV(_complexTourRequestsFilePath, complexTourRequests);
        }
    }
}
