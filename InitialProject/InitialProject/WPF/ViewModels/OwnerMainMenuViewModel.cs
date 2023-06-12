using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InitialProject.Application.Services;
using InitialProject.WPF.NewViews;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Shapes;
using System.Windows.Ink;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerMainMenuViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;
        private bool _isNotified;
        public bool IsNotified
        {
            get => _isNotified;
            set
            {
                if (value != _isNotified)
                {
                    _isNotified = value;
                    OnPropertyChanged(nameof(IsNotified));
                }
            }

        }
        public ICommand ViewProfileCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ViewForumsCommand { get; }
        public ICommand ViewHelpCommand { get; }
        public OwnerMainMenuViewModel(NavigationStore navigationStore, User user, bool isNotified)
        {
            _navigationStore = navigationStore;
            _user = user;
            IsNotified = isNotified;
            ViewProfileCommand = new ExecuteMethodCommand(ShowOwnerProfileView);
            SignOutCommand = new ExecuteMethodCommand(SignOut);
            ViewForumsCommand = new ExecuteMethodCommand(ShowForumSearchView);
            ViewHelpCommand = new ExecuteMethodCommand(ShowHelp);
        }

        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void ShowOwnerProfileView()
        {
            OwnerProfileViewModel ownerProfileViewModel = new OwnerProfileViewModel(_navigationStore, (Owner)_user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, ownerProfileViewModel));

            navigate.Execute(null);
        }
        private void ShowForumSearchView()
        {
            ForumSearchViewModel forumSearchViewModel = new ForumSearchViewModel(_navigationStore, (Owner)_user, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, forumSearchViewModel));

            navigate.Execute(null);
        }
        private void ShowHelp()
        {
            HelpView helpView = new HelpView();
            helpView.Show();
        }
    }
}
