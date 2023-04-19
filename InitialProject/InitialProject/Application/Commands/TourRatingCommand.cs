using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class TourRatingCommand : CommandBase
    {
        private readonly Action _execute;
        private User _user;
        private readonly TourReservationService _tourReservationService;

        public TourRatingCommand(Action execute, User user)
        {
            _user = user;
            _execute = execute;
            _tourReservationService = new TourReservationService();

            //_tourReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }


        public override bool CanExecute(object? parameter)
        {
            return _tourReservationService.GetUnratedByUser(_user.Id).FirstOrDefault() != null && base.CanExecute(parameter);
            //return !string.IsNullOrEmpty(_tourReservationViewModel.NumberOfGuests) && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
