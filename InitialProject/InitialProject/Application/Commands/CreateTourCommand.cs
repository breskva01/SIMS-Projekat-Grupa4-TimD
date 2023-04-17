using InitialProject.Application.Services;
using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Application.Commands
{
    public class CreateTourCommand : CommandBase
    {
        private readonly TourCreationViewModel viewModel;

        public CreateTourCommand(TourCreationViewModel tourCreationViewModel)
        {
            viewModel = tourCreationViewModel;
        }
        public override void Execute(object? parameter)
        {
            viewModel.CreateTour();
            
        }

    }
}
