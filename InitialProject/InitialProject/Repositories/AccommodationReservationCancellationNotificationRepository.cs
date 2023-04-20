using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationReservationCancellationNotificationRepository : IAccommodationReservationCancellationNotificationRepository
    {
        private readonly AccommodationReservationCancellationNotificationFileHandler _fileHandler;
        private List<AccommodationReservationCancellationNotification> _notifications;

        public AccommodationReservationCancellationNotificationRepository()
        {
            _fileHandler = new AccommodationReservationCancellationNotificationFileHandler();
            _notifications = _fileHandler.Load();
        }
        private void DeleteByOwnerId(int ownerId)
        {
            _notifications = _fileHandler.Load();
            _notifications.RemoveAll(n => n.OwnerId == ownerId);
            _fileHandler.Save(_notifications);
        }

        public List<AccommodationReservationCancellationNotification> GetByOwnerId(int ownerId)
        {
            _notifications = _fileHandler.Load();
            var notifications = _notifications.FindAll(n => n.OwnerId == ownerId);
            DeleteByOwnerId(ownerId);
            return notifications;
        }

        public void Save(AccommodationReservationCancellationNotification notification)
        {
            _notifications = _fileHandler.Load();
            _notifications.Add(notification);
            _fileHandler.Save(_notifications);
        }

        public List<AccommodationReservationCancellationNotification> GetAll()
        {
            return _fileHandler.Load();
        }
    }
}
