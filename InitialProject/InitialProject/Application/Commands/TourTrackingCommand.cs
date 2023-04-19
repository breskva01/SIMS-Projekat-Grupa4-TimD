﻿using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class TourTrackingCommand : CommandBase
    {
        private readonly Action _execute;
        private User _user;
        private readonly TourReservationService _tourReservationService;

        public TourTrackingCommand(Action execute, User user)
        {
            _user = user;
            _execute = execute;
            _tourReservationService = new TourReservationService();
        }

        public override bool CanExecute(object? parameter)
        {
            return _tourReservationService.GetActivePresent(_user.Id).FirstOrDefault() != null && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
