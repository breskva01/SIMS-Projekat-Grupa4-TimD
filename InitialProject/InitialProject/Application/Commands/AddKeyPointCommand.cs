using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class AddKeyPointCommand : CommandBase
    {
        private TourCreationViewModel viewModel;
        public AddKeyPointCommand(TourCreationViewModel tourCreationViewModel) 
        {
            viewModel = tourCreationViewModel;
        }
        public override void Execute(object? parameter)
        {
            viewModel.AddKeyPoint();
        }
    }
}
