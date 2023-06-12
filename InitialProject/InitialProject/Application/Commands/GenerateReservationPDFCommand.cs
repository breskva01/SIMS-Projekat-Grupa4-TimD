using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class GenerateReservationPDFCommand : CommandBase
    {
        private readonly Action<List<TourReservation>, User> _execute;

        public GenerateReservationPDFCommand(Action<List<TourReservation>, User> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is (List<TourReservation> myReservations, User user))
            {
                _execute(myReservations, user);
            }
        }
    }
}
