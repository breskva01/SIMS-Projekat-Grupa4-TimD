using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Commands
{
    public class UseVoucherCommand : CommandBase
    {
        private readonly Action<Voucher> _execute;

        public UseVoucherCommand(Action<Voucher> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is Voucher voucher)
            {
                _execute(voucher);
            }
        }
    }
}
