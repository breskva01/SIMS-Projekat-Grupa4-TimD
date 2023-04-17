using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class AccommodationReservationClickCommand : CommandBase
    {
        private readonly Action<AccommodationReservation> _execute;

        public AccommodationReservationClickCommand(Action<AccommodationReservation> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is AccommodationReservation reservation)
            {
                _execute(reservation);
            }
        }
    }
}
