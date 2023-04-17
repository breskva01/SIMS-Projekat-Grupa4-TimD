using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    class DecrementCommand : CommandBase
    {
        private readonly Action _execute;
        public DecrementCommand(Action execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
