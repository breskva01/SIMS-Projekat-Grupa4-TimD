using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class GuestRatingClickCommand : CommandBase
    {
        private readonly Action<GuestRating> _execute;

        public GuestRatingClickCommand(Action<GuestRating> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is GuestRating rating)
            {
                _execute(rating);
            }
        }
    }
}
