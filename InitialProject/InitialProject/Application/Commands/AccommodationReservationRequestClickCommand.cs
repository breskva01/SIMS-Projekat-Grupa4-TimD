using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class AccommodationReservationRequestClickCommand : CommandBase
    {
        private readonly Action<AccommodationReservationMoveRequest> _execute;

        public AccommodationReservationRequestClickCommand(Action<AccommodationReservationMoveRequest> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is AccommodationReservationMoveRequest request)
            {
                _execute(request);
            }
        }
    }
}
