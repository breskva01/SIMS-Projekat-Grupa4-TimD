using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.GuestOne;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class DecrementCommand : CommandBase
    {
        private readonly Func<int> _getValue;
        private readonly Action<int> _setValue;
        private readonly ViewModelBase _viewModel;

        public DecrementCommand(ViewModelBase viewModel, Func<int> getValue,
                                Action<int> setValue)
        {
            _getValue = getValue;
            _setValue = setValue;
            _viewModel = viewModel;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _getValue() > 1 && base.CanExecute(parameter);
        }
        public override void Execute(object? parameter)
        {
            int currentValue = _getValue();
            _setValue(currentValue - 1);
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "GuestCount" ||
               e.PropertyName == "NumberOfDays")
            {
                OnCanExecuteChanged(null);
            }
        }
    }
}
