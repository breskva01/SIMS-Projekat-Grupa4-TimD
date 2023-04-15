using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    class DecrementCommand : CommandBase, IDisposable
    {
        private readonly Action _execute;
        private readonly int _value;

        public DecrementCommand(Action execute, int value)
        {
            _execute = execute;
            _value = value;
        }
        public override bool CanExecute(object? parameter)
        {
            return _value > 1;
        }

        public void Dispose()
        {
            
        }

        public override void Execute(object? parameter)
        {
            _execute();
            Dispose();
        }
    }
}
