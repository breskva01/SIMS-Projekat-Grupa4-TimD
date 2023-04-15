using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Application.Commands
{
    public class SignInCommand : CommandBase
    {
        private readonly Action<string> _execute;
        public SignInCommand(Action<string> execute)
        {
            _execute = execute;
        }
        public override void Execute(object? parameter) { 
        
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;

            _execute(password);
        }

    }
}
