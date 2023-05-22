using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class UserNotificationFileHandler
    {
        private const string _userNotificationsFilePath = "../../../Resources/Data/userNotifications.csv";
        private readonly Serializer<UserNotification> _serializer;
        public UserNotificationFileHandler()
        {
            _serializer = new Serializer<UserNotification>();
        }
        public List<UserNotification> Load()
        {
            return _serializer.FromCSV(_userNotificationsFilePath);
        }
        public void Save(List<UserNotification> notifications)
        {
            _serializer.ToCSV(_userNotificationsFilePath, notifications);
        }
    }
}
