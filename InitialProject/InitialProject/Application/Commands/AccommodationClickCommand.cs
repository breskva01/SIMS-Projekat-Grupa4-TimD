using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class AccommodationClickCommand : CommandBase
    {
        private readonly Action<Accommodation> _execute;

        public AccommodationClickCommand(Action<Accommodation> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is Accommodation accommodation)
            {
                _execute(accommodation);
            }
        }
    }
}
