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
                    if (_selectedTabIndex > 1)
                        _selectedTabIndex = 0;
                    UpdateDisplayedForums();
                    OnPropertyChanged();
                }
            }
        }
        public ICommand NavigateStartForumFormCommand { get; }
        public ICommand NavigateForumCommentsCommand { get; }
        public ICommand CloseForumCommand { get; }
        public ForumBrowserViewModel(NavigationStore navigationStore, Guest1 user)
        {
            _user = user;
            _navigationStore = navigationStore;
            _forumService = new ForumService();
            Forums = new ObservableCollection<Forum>(_forumService.GetAll());
            NavigateStartForumFormCommand = new ExecuteMethodCommand(NavigateStartForumForm);
            CloseForumCommand = new ExecuteMethodCommand(CloseForum);
            NavigateForumCommentsCommand = new ExecuteMethodCommand(NavigateForumCommentsView);
        }
        private void CloseForum()
        {
            if (SelectedTabIndex != 1)
                return;
            if (SelectedForum == null)
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Izaberite forum koji želite zatvoriti.");
                else
                    MessageBox.Show("Please select the forum you'd like to close.");

            }
            else if (SelectedForum.Status == ForumStatus.Closed)
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Izabrani forum je već zatvoren.");
                else
                    MessageBox.Show("The chosen forum is already closed.");

            }
            else if (ConfirmClosing())
            {
                _forumService.Close(SelectedForum);
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Forum uspešno zatvoren.");
                else
                    MessageBox.Show("Forum successfuly closed.");
                UpdateDisplayedForums();
            }
        }
        private bool ConfirmClosing()
        {
            string messageBoxText = "";
            string messageBoxCaption = "";
            if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
            {
                messageBoxText = "Jednom zatvoren forum ostaje zauvek zatvoren.\n" +
                 "Već postavljeni komentari će i dalje biti vidljivi svim korisnicima.\n" +
                 "Da li ste sigurni da želite zatvoriti forum?\n";
                messageBoxCaption = "Potvrda zatvaranja foruma";
            }
            else
            {
                messageBoxText = "Once closed, the forum will remain closed forever.\n" +
                 "Existing comments will still be visible to all users.\n" +
                 "Are you sure you want to close the forum?\n";
                messageBoxCaption = "Forum Closing Confirmation";
            }
            MessageBoxResult result = MessageBox.Show(messageBoxText, messageBoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
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
        private void NavigateForumCommentsView()
        {
            if (SelectedForum == null)
            {
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                    MessageBox.Show("Izaberite forum čije komentare želite otvoriti.");
                else
                    MessageBox.Show("Select the forum for which you want to open the comments.");
            }
            else
            {
                var viewModel = new ForumCommentsViewModel(_navigationStore, _user, SelectedForum);
                new NavigationService(_navigationStore, viewModel).Navigate();
            }
        }
    }
}
