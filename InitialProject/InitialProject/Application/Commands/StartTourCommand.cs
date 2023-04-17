using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class StartTourCommand : CommandBase
    {
        private ToursTodayViewModel viewModel;

        public StartTourCommand(ToursTodayViewModel toursTodayViewModel) 
        {
            viewModel = toursTodayViewModel;
        }

        public override void Execute(object? parameter)
        {
            //viewModel.StartTour();
        }
    }
}
