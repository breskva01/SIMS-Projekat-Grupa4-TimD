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
    public class MakeReservationCommand : CommandBase

    {
        private readonly TourReservationViewModel _tourReservationViewModel;
        private readonly NavigationService tourBrowserNavigationService;
        public MakeReservationCommand(TourReservationViewModel tourReservationViewModel, NavigationService tourBrowserNavigationService) 
        {
            _tourReservationViewModel = tourReservationViewModel;

            this.tourBrowserNavigationService = tourBrowserNavigationService;
            _tourReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object? parameter)
        {
            tourBrowserNavigationService.Navigate();
        }
    }
}
