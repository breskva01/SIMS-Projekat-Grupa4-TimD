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
    public class ForumBrowserViewModel : ViewModelBase
    {
        private readonly Guest1 _user;
        private readonly NavigationStore _navigationStore;
        public ObservableCollection<Forum> Forums { get; set; }
        public Forum SelectedForum { get; set; }
        private readonly ForumService _forumService;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (value != _selectedTabIndex)
                {
                    _selectedTabIndex = value;
                    UpdateDisplayedForums();
                    OnPropertyChanged();
                }
            }
        }
        public ICommand NavigateStartForumFormCommand { get; }
        public ICommand CloseForumCommand { get; }
        public ForumBrowserViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _forumService = new ForumService();
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
            NavigateStartForumFormCommand = new ExecuteMethodCommand(NavigateStartForumForm);
            CloseForumCommand = new ExecuteMethodCommand(CloseForum);
        }
        private void CloseForum()
        {
            if (SelectedForum == null)
                MessageBox.Show("Izaberite forum koji želite zatvoriti");
            else
            {
                _forumService.Close(SelectedForum);
                MessageBox.Show("Forum uspešno zatvoren");
                UpdateDisplayedForums();
            }
            
        }
        private void UpdateDisplayedForums()
        {
            Forums.Clear();
            SelectedForum = null;
            switch (_selectedTabIndex)
            {
                case 0:
                    _forumService.GetAll().ForEach(f => Forums.Add(f));
                    break;
                case 1:
                    _forumService.GetByIniatorId(_user.Id).ForEach(f => Forums.Add(f));
                    break;
            }
        }
        private void NavigateStartForumForm()
        {
            var viewModel = new StartForumViewModel(_navigationStore, _user);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }
}
