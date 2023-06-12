using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        private readonly UserNotificationService _userNotificationService;
        public ObservableCollection<UserNotification> Notifications { get; set; }
        private User _owner;
        public NotificationsViewModel(User owner)
        {
            _owner = owner;
            _userNotificationService = new UserNotificationService();
            Notifications = new ObservableCollection<UserNotification>(_userNotificationService.GetByUser(_owner.Id));
            _owner = owner;
        }
    }
}
