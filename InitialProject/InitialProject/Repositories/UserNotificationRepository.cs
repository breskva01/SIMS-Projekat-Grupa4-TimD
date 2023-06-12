﻿using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            List<UserNotification> userNotifications = new List<UserNotification>();
            foreach (UserNotification un in notifications.ToList())
            {
                if (un.UserId == userId)
                {
                    userNotifications.Add(un);
                }
            }
            return userNotifications;
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

        public void NotifySimilarRequests(Tour tour, List<TourRequest> requests)
        {
            foreach (TourRequest request in requests)
            {
                if(request.Language == tour.Language || request.Location == tour.Location)
                {
                    UserNotification notification = new UserNotification();
                    notification.UserId = request.UserId;
                    notification.Message = "Tour " + tour.Name + ", similar to a request you've made has just been created." +
                        "Location: " + tour.Location.City + " - " + tour.Location.Country + "" +
                        "Duration: " + tour.Duration.ToString()  +
                        "Date: " + tour.Start.ToString("dd-MM-yyyy") + 
                        "Language: " + tour.Language + 
                        "If you want to reserve spots on the tour, check MainMenu/TourReservation";

                    notification.Time = DateTime.Now;
                    Save(notification);
                }
            }
        }

        public void NotifyApprovedRequest(Tour tour, int userId)
        {
            UserNotification notification = new UserNotification();
            notification.UserId = userId;
            notification.Message = "Tour " + tour.Location.City + " - " + tour.Location.Country + " that you requested has been approved." +
                "For more details go to: " +
                "MyInfo/Requests";
            notification.Time = DateTime.Now;
            Save(notification);
        }

        public void NotifyFreeVoucher(int guestId)
        {
            UserNotification notification = new UserNotification();
            notification.UserId = guestId;
            notification.Message = "You have been awarded a free Voucher for attending 5 tourist tours. The voucher can be used in the next 6 months.";
            notification.Time = DateTime.Now;
            Save(notification);
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
        public void UpdateUnreadNotifications(int id)
        {
            _notifications = _userNotificationFileHandler.Load();
            foreach(UserNotification notification in _notifications.ToList())
            {
                if(notification.UserId == id && !notification.IsRead)
                {
                    _notifications.Remove(notification);
                    notification.IsRead = true;
                    _notifications.Add(notification);
                }
            }
            _userNotificationFileHandler.Save(_notifications);
        }
    }
}
