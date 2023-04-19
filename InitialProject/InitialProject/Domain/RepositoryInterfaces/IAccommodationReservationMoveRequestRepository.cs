using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationMoveRequestRepository
    {
        List<AccommodationReservationMoveRequest> GetAll();
        List<AccommodationReservationMoveRequest> GetByOwnerId(int ownerId);
        List<AccommodationReservationMoveRequest> GetByGuestId(int guestId);
        public List<AccommodationReservationMoveRequest> GetPendingRequestsByOwnerId(int ownerId);
        public void ApproveRequest(int reservationId);
        public void DenyRequest(int reservationId, string comment);
        List<AccommodationReservationMoveRequest> GetAllNewlyAnswered(int guestId);
        void UpdateGuestNotifiedField(int guestId);
        void Save(AccommodationReservationMoveRequest request);
    }

}
