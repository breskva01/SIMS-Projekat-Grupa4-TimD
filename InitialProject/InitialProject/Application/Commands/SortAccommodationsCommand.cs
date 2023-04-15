using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class SortAccommodationsCommand : CommandBase
    {
        private readonly Action<string> _execute;
        private readonly string _sortCriterium;
        public SortAccommodationsCommand(Action<string> execute, string sortCriterium)
        {
            _execute = execute;
            _sortCriterium = sortCriterium;
        }

        public override void Execute(object? parameter)
        {
            _execute(_sortCriterium);
        }
    }
}
