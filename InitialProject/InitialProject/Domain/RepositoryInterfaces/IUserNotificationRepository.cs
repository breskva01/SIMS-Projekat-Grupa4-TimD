using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        public UserNotification GetById(int notificationId);
        public List<UserNotification> GetByUser(int userId);
        public UserNotification Update(UserNotification notification);
        public UserNotification Save(UserNotification notification);
        public void NotifyApprovedRequest(Tour tour, int userId);
        public void NotifySimilarRequests(Tour tour, List<TourRequest> requests);
        public int NextId();
        public void Delete(UserNotification notification);
        public void UpdateUnreadNotifications(int id);
    }
}
