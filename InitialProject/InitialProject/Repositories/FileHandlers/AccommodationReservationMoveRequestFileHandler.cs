using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class AccommodationReservationMoveRequestFileHandler
    {
        private const string _requestsFilePath = "../../../Resources/Data/accommodationReservationMoveRequests.csv";
        private readonly Serializer<AccommodationReservationMoveRequest> _serializer;

        public AccommodationReservationMoveRequestFileHandler()
        {
            _serializer = new Serializer<AccommodationReservationMoveRequest>();
        }
        public List<AccommodationReservationMoveRequest> Load()
        {
            var requests = _serializer.FromCSV(_requestsFilePath);
            FillInReservations(requests);
            return requests;
        }
        private void FillInReservations(List<AccommodationReservationMoveRequest> requests)
        {
            var reservations = new AccommodationReservationFileHandler().Load();
            requests.ForEach(r =>
                r.Reservation = reservations.Find(res => res.Id == r.Reservation.Id));
        }
        public void Save(List<AccommodationReservationMoveRequest> requests)
        {
            _serializer.ToCSV(_requestsFilePath, requests);
        }
    }
}
