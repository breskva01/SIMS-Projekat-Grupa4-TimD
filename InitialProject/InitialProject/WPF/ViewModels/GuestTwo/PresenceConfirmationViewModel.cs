using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class PresenceConfirmationViewModel : ViewModelBase
    {
        private User _user;
        private Tour _tour;
        private readonly NavigationStore _navigationStore;
        private TourReservation _pendingReservation;
        private readonly UserService _userService;
        private readonly TourReservationService _tourReservationService;
        private readonly TourService _tourService;
        private readonly VoucherService _voucherService;
        public string TourName { get; set; }

        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public PresenceConfirmationViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _userService = new UserService();

            _pendingReservation = _tourReservationService.GetActivePending(_user.Id).FirstOrDefault();
            _tour = _tourService.GetById(_pendingReservation.TourId);
            TourName = _tour.Name;

            YesCommand = new ExecuteMethodCommand(ConfirmPresence);
            NoCommand = new ExecuteMethodCommand(DenyPresence);

        }

        ViewModelBase DefineNextView()
        {
            if (_tourReservationService.GetActivePending(_user.Id).Any())
            {
                return new PresenceConfirmationViewModel(_navigationStore, _user);
            }
            return new Guest2MenuViewModel(_navigationStore, _user);
        }

        private void DenyPresence()
        {
            _pendingReservation.Presence = Presence.Absent;
            _pendingReservation.ArrivedAtKeyPoint = 0;
            _tourReservationService.Update(_pendingReservation);

            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, DefineNextView()));
            navigate.Execute(null);
        }

        private void ConfirmPresence()
        {
            List<TourReservation> duplicateReservations = _tourReservationService.GetDuplicates(_user.Id, _tour.Id);
            foreach(TourReservation tr in duplicateReservations)
            {
                tr.Presence = Presence.Present;
                _tour.NumberOfArrivedGeusts += tr.NumberOfGuests;
                _tourReservationService.Update(tr);
            }
            _tourService.Update(_tour);

            _userService.CheckVoucherProgress((Guest2) _user);

            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, DefineNextView()));
            navigate.Execute(null);

        }
    }
}
