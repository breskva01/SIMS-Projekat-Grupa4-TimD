using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationReservationRequestService
    {
        private readonly IAccommodationReservationMoveRequestRepository _repository;
        public AccommodationReservationRequestService()
        {
            _repository = RepositoryStore.GetIAccommodationReservationMoveRequestRepository;
        }
        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _repository.GetAll();
        }
        public List<AccommodationReservationMoveRequest> GetByOwnerId(int ownerId)
        {
            return _repository.GetByOwnerId(ownerId);
        }
        public List<AccommodationReservationMoveRequest> GetByGuestId(int guestId)
        {
            return _repository.GetByGuestId(guestId);
        }
        public void Save(AccommodationReservation reservation, DateOnly requestedCheckIn, DateOnly requestedCheckOut)
        {
            var request = new AccommodationReservationMoveRequest(reservation, requestedCheckIn, requestedCheckOut);
            _repository.Save(request);
        }
    }
}
