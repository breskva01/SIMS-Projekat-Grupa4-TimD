using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class CurrentKeyPointViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        private readonly KeyPointService _keyPointService;
        public string KeyPointPlace { get; set; }
        public string TourName { get; set; }

        public ICommand MenuCommand { get; }
        
        public CurrentKeyPointViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _keyPointService = new KeyPointService();

            Tour trackedTour = _tourService.GetById(_tourReservationService.GetActivePresent(_user.Id).FirstOrDefault().TourId);


            TourName = trackedTour.Name;
            KeyPointPlace = _keyPointService.GetById(trackedTour.CurrentKeyPoint).Place;

            MenuCommand = new ExecuteMethodCommand(ShowGuest2Menu);

        }

        private void ShowGuest2Menu()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }
    }

}
