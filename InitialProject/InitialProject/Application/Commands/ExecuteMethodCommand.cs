using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class ExecuteMethodCommand : CommandBase
    {
        private readonly Action _execute;

        public ExecuteMethodCommand(Action execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            _execute();
        }
    }
}
