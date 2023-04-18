using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    class ReserveWithVoucherCommand : CommandBase
    {
        private readonly Action _execute;
        private readonly TourReservationViewModel _tourReservationViewModel;

        public ReserveWithVoucherCommand(Action execute, TourReservationViewModel tourReservationViewModel)
        {
            _tourReservationViewModel = tourReservationViewModel;
            _execute = execute;

            _tourReservationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        
        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_tourReservationViewModel.NumberOfGuests) && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            _execute();
        }
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(TourReservationViewModel.NumberOfGuests))
            {
                OnCanExecuteChanged(null);
            }
        }

    }
}
