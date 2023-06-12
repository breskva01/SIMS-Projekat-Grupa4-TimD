using ControlzEx.Standard;
using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class UserNotificationService
    {
        private readonly List<IObserver> _observers;
        private readonly IUserNotificationRepository _repository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IUserRepository _userRepository;
        private readonly TourRequestService _tourRequestService;
        public UserNotificationService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<IUserNotificationRepository>();
            _accommodationRepository = RepositoryInjector.Get<IAccommodationRepository>();
            _userRepository = RepositoryInjector.Get<IUserRepository>();
            _tourRequestService = new TourRequestService();
        }

        public List<UserNotification> GetAll()
        {
            return _repository.GetAll();
        }

        public void NotifySimilarRequests(Tour tour)
        {
            _repository.NotifySimilarRequests(tour, _tourRequestService.GetOnHold());
        }

        public void NotifyApprovedRequest(Tour tour, int userId)
        {
            _repository.NotifyApprovedRequest(tour, userId);
        }

        public void NotifyFreeVoucher(int guestId)
        {
            _repository.NotifyFreeVoucher(guestId);
        }

        public UserNotification GetById(int notificationId)
        {
            return _repository.GetById(notificationId);
        }
        public List<UserNotification> GetByUser(int userId)
        {
            return _repository.GetByUser(userId);
        }

        public UserNotification CreateNotification(int UserId, string Message, DateTime Time)
        {
            UserNotification Notification = new UserNotification();
            Notification.UserId = UserId;
            Notification.Message = Message;
            Notification.Time = Time;

            return _repository.Save(Notification);
        }
        public UserNotification Update(UserNotification notification)
        {
            return _repository.Update(notification);
        }
        public void OpenedForumNotification(Location location, string topic)
        {
            List<User> users = _userRepository.GetAll();
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            foreach (User user in users)
            {
                foreach (Accommodation accommodation in accommodations)
                {
                    if (accommodation.Owner.Id == user.Id && accommodation.Location == location)
                    {
                        CreateNotification(user.Id, "A new forum " + topic + " for the location " + location.Country + " - " + location.City + " has opened. Go check it out!", DateTime.Now);
                    }
                }
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
        public void UpdateUnreadNotifications(int id)
        {
            _repository.UpdateUnreadNotifications(id);
        }
    }
}
