using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class ComplexRequestClickCommand : CommandBase
    {
        private readonly Action<ComplexTourRequest> _execute;

        public ComplexRequestClickCommand(Action<ComplexTourRequest> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is ComplexTourRequest selectedRequest)
            {
                _execute(selectedRequest);
            }
        }
    }
}
