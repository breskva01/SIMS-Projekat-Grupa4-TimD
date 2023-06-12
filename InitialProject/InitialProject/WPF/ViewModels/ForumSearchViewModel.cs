using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class ForumSearchViewModel : ViewModelBase
    {
        private ForumService _forumService;
        public List<string> Forums { get; set; }
        public string SelectedLocationAndTopic { get; set; }
        public Forum Forum { get; set; }
        private NavigationStore _navigationStore;
        private Owner _owner;
        public bool IsNotified;
        public ICommand BackCommand { get; set; }
        public ICommand ViewForumCommand { get; set; }
        public ForumSearchViewModel(NavigationStore navigationStore, Owner owner, bool isNotified) 
        {
            _navigationStore = navigationStore;
            _owner = owner;
            IsNotified = isNotified;
            _forumService = new ForumService();
            Forums = new List<string>(_forumService.GetForumNames());
            BackCommand = new ExecuteMethodCommand(Back);
            ViewForumCommand = new ExecuteMethodCommand(ViewForumComments);
        }

        private void Back() 
        {
            OwnerMainMenuViewModel ownerMainMenuViewModel = new OwnerMainMenuViewModel(_navigationStore, _owner, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, ownerMainMenuViewModel));

            navigate.Execute(null);
        }
        private void ViewForumComments()
        {
            Forum = _forumService.GetForumByLocationAndTopic(SelectedLocationAndTopic);
            ForumCommentsViewModel forumCommentsViewModel = new ForumCommentsViewModel(_navigationStore, _owner, IsNotified, Forum);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, forumCommentsViewModel));

            navigate.Execute(null);
        }
    }
}
