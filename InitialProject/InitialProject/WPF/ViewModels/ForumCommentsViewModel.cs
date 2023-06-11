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

namespace InitialProject.WPF.ViewModels
{
    public class ForumCommentsViewModel: ViewModelBase
    {
        private ForumService _forumService;
        private NavigationStore _navigationStore;
        private Owner _owner;
        public bool IsNotified;
        public Forum SelectedForum { get; set; }
        public ObservableCollection<Comment> Comments { get; set;}
        public Comment SelectedComment { get; set; }
        public ICommand BackCommand { get; }
        public ICommand SubmitCommentCommand { get; }
        public ICommand ReportCommentCommand { get; }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set 
            {
                if (_comment != value) 
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        public ForumCommentsViewModel(NavigationStore navigationStore, Owner owner, bool isNotified, Forum selectedForum)
        {
            _navigationStore = navigationStore;
            _owner = owner;
            IsNotified = isNotified;
            SelectedForum = selectedForum;
            _forumService = new ForumService();
            Comments = new ObservableCollection<Comment>(_forumService.GetCommentsByForumId(SelectedForum.Id));
            var SortedComments =  Comments.OrderBy(c => c.PostTime);
            BackCommand = new ExecuteMethodCommand(Back);
            SubmitCommentCommand = new ExecuteMethodCommand(SubmitComment);
            ReportCommentCommand = new ExecuteMethodCommand(ReportComment);
        }
        private void Back()
        {
            ForumSearchViewModel forumSearchViewModel = new ForumSearchViewModel(_navigationStore, _owner, IsNotified);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, forumSearchViewModel));

            navigate.Execute(null);
        }
        private void SubmitComment() 
        {
            Comment comment = _forumService.SubmitComment(SelectedForum, Comment, _owner);
            if(!comment.CredentialAuthor)
            {
                MessageBox.Show("You do not own an accommodation on this location!");
                Comment = "";
                return;
            }
            Comment = "";
            Comments.Add(comment);
        }
        private void ReportComment()
        {
            string retVal = _forumService.ReportComment(SelectedComment, _owner);
            if(retVal == "true") 
            {
                MessageBox.Show("You have already reported this comment!");
            }
            else if(retVal == "This user was verified on this location!")
            {
                MessageBox.Show(retVal);
            }
            else if(retVal == "This user is an owner on this location!")
            {
                MessageBox.Show(retVal);
            }
            Comments.Clear();
            foreach (var comment in _forumService.GetComments())
            {
                Comments.Add(comment);
                var SortedComments = Comments.OrderBy(c => c.PostTime);
            }
        }
    }
}
