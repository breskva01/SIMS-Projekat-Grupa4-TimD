using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class ImageClickCommand : CommandBase
    {
        private readonly Action<string> _execute;

        public ImageClickCommand(Action<string> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is string imageURL)
            {
                _execute(imageURL);
            }
        }
    }
}
