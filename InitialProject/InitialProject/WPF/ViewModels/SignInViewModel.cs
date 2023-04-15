using InitialProject.Application.Services;
using InitialProject.Application.Storage;
using InitialProject.Controller;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly UserService _service;
        public SecureString Password { get; set; }
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }
        public SignInViewModel()
        {
            _service = new UserService();         
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _service.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password.Equals(Password))
                {
                    OpenAppropriateWindow(user);
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }
        private void OpenAppropriateWindow(User user)
        {
            switch (user.Type)
            {
                // TO DO: otvoriti odgovarajuce prozore za svaki tip korisnika
                case UserType.Owner:
                    {
                        break;
                    }
                case UserType.Guest1:
                    {
                        break;
                    }
                case UserType.TourGuide:
                    {
                        break;
                    }
                case UserType.Guest2:
                    {
                        break;
                    }
            }
        }

    }
}
