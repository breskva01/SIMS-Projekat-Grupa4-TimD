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
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly UserNotificationFileHandler _userNotificationFileHandler;
        private readonly UserFileHandler _userFileHandler;
        private List<UserNotification> _notifications;

        public UserNotificationRepository()
        {
            _userNotificationFileHandler = new UserNotificationFileHandler();
            _userFileHandler = new UserFileHandler();
            _notifications = _userNotificationFileHandler.Load();
        }

        public void Delete(UserNotification notification)
        {
            _notifications = _userNotificationFileHandler.Load();
            UserNotification founded = _notifications.Find(a => a.Id == notification.Id);
            _notifications.Remove(founded);
            _userNotificationFileHandler.Save(_notifications);
        }

        public List<UserNotification> GetAll()
        {
            return _userNotificationFileHandler.Load();
        }

        public UserNotification GetById(int notificationId)
        {
            _notifications = _userNotificationFileHandler.Load();
            return _notifications.Find(v => v.Id == notificationId);
        }

        public List<UserNotification> GetByUser(int userId)
        {
            List<UserNotification> notifications = GetAll();
            foreach (UserNotification un in notifications)
            {
                if (un.UserId == userId)
                {
                    notifications.Add(un);
                }
            }
            return notifications;
        }

        public int NextId()
        {
            _notifications = _userNotificationFileHandler.Load();
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(t => t.Id) + 1;
        }

        public UserNotification Save(UserNotification notification)
        {
            notification.Id = NextId();
            _notifications = _userNotificationFileHandler.Load();
            _notifications.Add(notification);
            _userNotificationFileHandler.Save(_notifications);
            return notification;
        }

        public UserNotification Update(UserNotification notification)
        {
            _notifications = _userNotificationFileHandler.Load();
            UserNotification updated = _notifications.Find(t => t.Id == notification.Id);
            _notifications.Remove(notification);
            _notifications.Add(notification);
            _userNotificationFileHandler.Save(_notifications);
            return notification;
        }
    }
}
