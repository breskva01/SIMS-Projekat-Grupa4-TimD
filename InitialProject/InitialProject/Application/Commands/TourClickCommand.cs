using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class TourClickCommand : CommandBase

    {
        private readonly Action<Tour> _execute;

        public TourClickCommand(Action<Tour> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is Tour tour)
            {
                _execute(tour);
            }
        }
    }
}
