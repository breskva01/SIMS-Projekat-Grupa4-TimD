using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class AccommodationReservationCancellationNotificationFileHandler
    {
        private const string _notificationsFilePath = "../../../Resources/Data/accommodationReservationNotifications.csv";
        private readonly Serializer<AccommodationReservationCancellationNotification> _serializer;

        public AccommodationReservationCancellationNotificationFileHandler()
        {
            _serializer = new Serializer<AccommodationReservationCancellationNotification>();
        }
        public List<AccommodationReservationCancellationNotification> Load()
        {
            return _serializer.FromCSV(_notificationsFilePath);
        }
        public void Save(List<AccommodationReservationCancellationNotification> notifications)
        {
            _serializer.ToCSV(_notificationsFilePath, notifications);
        }
    }
}
