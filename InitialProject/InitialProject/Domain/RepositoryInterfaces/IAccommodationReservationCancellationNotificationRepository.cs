using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationCancellationNotificationRepository
    {
        List<AccommodationReservationCancellationNotification> GetByOwnerId(int ownerId);
        void Save(AccommodationReservationCancellationNotification notification);
    }
}
