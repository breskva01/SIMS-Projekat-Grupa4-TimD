using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class ForumCommentsViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public Forum Forum { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }
        private readonly ForumService _forumService;
        private string _commentText;
        public string CommentText
        {
            get { return _commentText; }
            set
            {
                if (value != _commentText)
                {
                    _commentText = value;
                    OnPropertyChanged();
                }
            }
        }
        public string UserName => _user.FirstName;
        public ICommand NavigateBackCommand { get; }
        public ICommand PostCommentCommand { get; }
        public ForumCommentsViewModel(NavigationStore navigationStore, Guest1 user, Forum forum)
        {
            _user = user;
            _navigationStore = navigationStore;
            Forum = forum;
            _forumService = new ForumService();
            NavigateBackCommand = new ExecuteMethodCommand(NavigateForumBrowser);
            PostCommentCommand = new ExecuteMethodCommand(PostComment);
            Comments = new ObservableCollection<Comment>(Forum.Comments);
        }
        private void PostComment()
        {
            if (string.IsNullOrEmpty(CommentText))
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Ne možete postaviti prazan komentar.");
                else
                    MessageBox.Show("An empty comment can not be posted.");

            }
            else
            {
                _forumService.PostComment(Forum, _user, CommentText);
                //MessageBox.Show("Komentar uspešno postavljen.");
                CommentText = "";
                UpdateComments();
            }
        }
        private void UpdateComments()
        {
            Comments.Clear();
            Forum = _forumService.GetById(Forum.Id);
            Forum.Comments.ForEach(c => Comments.Add(c));
        }
        private void NavigateForumBrowser()
        {
            ViewModelBase contentViewModel = new ForumBrowserViewModel(_navigationStore, _user);
            var navigateBarViewModel = new NavigationBarViewModel(_navigationStore, _user);
            var layoutViewModel = new LayoutViewModel(navigateBarViewModel, contentViewModel);
            new NavigationService(_navigationStore, layoutViewModel).Navigate();
        }
    }
}
