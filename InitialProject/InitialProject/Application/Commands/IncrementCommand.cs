using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class IncrementCommand : CommandBase
    {
        private readonly Func<int> _getValue;
        private readonly Action<int> _setValue;

        public IncrementCommand(Func<int> getValue, Action<int> setValue)
        {
            _getValue = getValue;
            _setValue = setValue;
        }

        public override void Execute(object? parameter)
        {
            int currentValue = _getValue();
            _setValue(currentValue + 1);
        }
    }

}
