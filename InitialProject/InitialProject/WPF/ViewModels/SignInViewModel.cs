﻿using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Storage;
using InitialProject.Application.Stores;
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
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        
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

        public ICommand SignInCommand { get; }
        public ICommand Guest2NavigateCommand { get; }
        public ICommand Guest1NavigateCommand { get; }
        public SignInViewModel(NavigationStore navigationStore, Func<ViewModelBase> createViewModelGuest2)
        {

            _userService = new UserService();       
            SignInCommand = new SignInCommand(SignIn);
            //Guest1NavigateCommand = new NavigateCommand(new NavigationService(navigationStore, createViewModelGuest1));
            Guest2NavigateCommand = new NavigateCommand(new NavigationService(navigationStore, createViewModelGuest2));

        }

        private void SignIn(String password)
        {
            User user = _userService.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password.Equals(password))
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
                        //Guest1NavigateCommand.Execute(null);
                        break;
                    }
                case UserType.TourGuide:
                    {
                        break;
                    }
                case UserType.Guest2:
                    {
                        Guest2NavigateCommand.Execute(null);
                        break;
                    }
            }
        }

    }
}
