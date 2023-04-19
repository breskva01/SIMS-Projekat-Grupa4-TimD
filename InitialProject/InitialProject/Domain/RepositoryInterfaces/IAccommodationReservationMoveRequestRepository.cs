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
        List<AccommodationReservationMoveRequest> GetAllNewlyAnswered(int guestId);
        void Save(AccommodationReservationMoveRequest request);
    }

}
