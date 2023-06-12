using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class RateTourViewModel : ViewModelBase
    {
        private User _user;
        private Tour _tour;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly NavigationStore _navigationStore;

        public int Duration = 20;


        private int _knowledgeRating;
        public int KnowledgeRating
        {
            get => _knowledgeRating;
            set
            {
                if (value != _knowledgeRating)
                {
                    _knowledgeRating = value;
                    OnPropertyChanged(nameof(KnowledgeRating));
                }
            }
        }

        private int _languageRating;
        public int LanguageRating
        {
            get => _languageRating;
            set
            {
                if (value != _languageRating)
                {
                    _languageRating = value;
                    OnPropertyChanged(nameof(LanguageRating));
                }
            }
        }

        private int _interestingRating;
        public int InterestingRating
        {
            get => _interestingRating;
            set
            {
                if (value != _interestingRating)
                {
                    _interestingRating = value;
                    OnPropertyChanged(nameof(InterestingRating));
                }
            }
        }

        private int _informativeRating;
        public int InformativeRating
        {
            get => _informativeRating;
            set
            {
                if(value!= _informativeRating)
                {
                    _informativeRating = value;
                    OnPropertyChanged(nameof(InformativeRating));
                }
            }
        }

        private int _contentRating;
        public int ContentRating
        {
            get => _contentRating;
            set
            {
                if(value != _contentRating)
                {
                    _contentRating = value;
                    OnPropertyChanged(nameof(ContentRating));
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }
        private List<string> _pictureURLs;
        private List<string> _selectedFiles;

        private string _demoMessage;
        public string DemoMessage
        {
            get => _demoMessage;
            set
            {
                if (value != _demoMessage)
                {
                    _demoMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isDemo;
        public bool IsDemo
        {
            get => _isDemo;
            set
            {
                if (value != _isDemo)
                {
                    _isDemo = value;
                    if(value == true)
                    {
                        DemoMessage = "Click here to stop the demo";
                        StopDemo = false;
                        OnPropertyChanged(nameof(StopDemo));
                        OnPropertyChanged(nameof(DemoMessage));
                    }
                    OnPropertyChanged();
                }
            }
        }
        private bool _stopDemo;
        public bool StopDemo
        {
            get => _stopDemo;
            set
            {
                if (value != _stopDemo)
                {
                    _stopDemo = value;
                    if(StopDemo == true)
                    {
                        DemoMessage = "Take a look of the quick demo of this functionality";
                        IsDemo = false;
                        OnPropertyChanged(nameof(StopDemo));
                        OnPropertyChanged(nameof(DemoMessage));
                    }
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEnabledElement;
        public bool IsEnabledElement
        {
            get { return _isEnabledElement; }
            set
            {
                _isEnabledElement = value;
                OnPropertyChanged(nameof(IsEnabledElement));
            }
        }


        public ICommand BackCommand { get; }
        public ICommand RateCommand { get; }
        public ICommand UploadImagesCommand { get; }
        public ICommand NotificationCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand DemoCommand { get; }

        public RateTourViewModel(NavigationStore navigationStore, User user, Tour tour) {
            _navigationStore = navigationStore;
            _user = user;
            _tour = tour;
            _tourReservationService = new TourReservationService();
            _tourRatingService = new TourRatingService();
            _pictureURLs = new List<string>();

            IsDemo = false;
            StopDemo = false;
            DemoMessage = "Take a look of the quick demo of this functionality";
            IsEnabledElement = true;


            DemoCommand = new ExecuteMethodCommand(DoTheDemoCommand);
            BackCommand = new ExecuteMethodCommand(ShowTourRatingView);
            RateCommand = new ExecuteMethodCommand(SubmitRating);
            UploadImagesCommand = new ExecuteMethodCommand(UploadImages);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationBrowserView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            
        }

        public void DoTheDemoCommand()
        {
            if (IsDemo)
            {
                StopDemo = true;
                return;
            }
            IsDemo = true;
            StartDemoAsync();
        }

        public async void StartDemoAsync()
        {
            Empty();
            Disable();
            IsDemo = true;

            while (true)
            {
                if (StopDemo) break;
                await StartGuideKnowledgeAnimation();
                if (StopDemo) break;
                await StartGuideLanguageAnimation();
                if (StopDemo) break;
                await StartTourInterestingAnimation();
                if (StopDemo) break;
                await StartTourInformativeAnimation();
                if (StopDemo) break;
                await StartTourContentAnimation();
                if (StopDemo) break;
                await StartCommentAnimation();
                if (StopDemo) break;
                await ClearAll();
                if (StopDemo) break;
            }
            IsDemo = false;
            StopDemo = false;
            Empty();
            Enable();
        }

        private void Empty()
        {
            KnowledgeRating = 1;
            LanguageRating = 1;
            InterestingRating = 1;
            InformativeRating = 1;
            ContentRating = 1;
            Comment = string.Empty;
        }

        private void Disable()
        {
            IsEnabledElement = false;
        }

        private void Enable()
        {
            IsEnabledElement = true;
        }

        private async Task StartGuideKnowledgeAnimation()
        {
            int targetGrade = 3;
            for(int i = 1; i < targetGrade; i++)
            {
                KnowledgeRating++;
                await Task.Delay(Duration*10);
            }

            await Task.Delay(Duration * 10);
        }

        private async Task StartGuideLanguageAnimation()
        {
            int targetGrade = 5;
            for (int i = 1; i < targetGrade; i++)
            {
                LanguageRating++;
                await Task.Delay(Duration * 10);
            }

            await Task.Delay(Duration * 10);
        }

        private async Task StartTourInformativeAnimation()
        {
            int targetGrade = 3;
            for (int i = 0; i < targetGrade; i++)
            {
                InformativeRating++;
                await Task.Delay(Duration * 10);
            }

            await Task.Delay(Duration * 10);
        }

        private async Task StartTourContentAnimation()
        {
            int targetGrade = 4;
            for (int i = 1; i < targetGrade; i++)
            {
                ContentRating++;
                await Task.Delay(Duration * 10);
            }

            await Task.Delay(Duration * 10);
        }

        private async Task StartTourInterestingAnimation()
        {
            int targetGrade = 4;
            for (int i = 1; i < targetGrade; i++)
            {
                InterestingRating++;
                await Task.Delay(Duration * 10);
            }

            await Task.Delay(Duration * 10);
        }

        

        private async Task StartCommentAnimation()
        {
            string targetText = "This tour was actually pretty fun. Only thing that it lacked, was some more educational content.";

            foreach (char c in targetText)
            {
                Comment += c;
                await Task.Delay(Duration * 2);
            }

            await Task.Delay(Duration * 20);
        }

        private async Task ClearAll()
        {
            Empty();
            await Task.Delay(Duration * 10);
        }

        public void SubmitRating()
        {
            if (!IsEnabledElement)
            {
                return;
            }
            CopyImages();
            TourRating rating = _tourRatingService.Save(KnowledgeRating, LanguageRating, InterestingRating, InformativeRating,
                          ContentRating, Comment, _pictureURLs);
            List<TourReservation> reservations = _tourReservationService.GetByUserAndTourId(_user.Id, _tour.Id);
            foreach(TourReservation r in reservations)
            {
                r.RatingId = rating.Id;
                _tourReservationService.Update(r);
            }

            ShowTourRatingView();

        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }


        private void ShowNotificationBrowserView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));
            navigate.Execute(null);
        }

        private void ShowTourRatingView()
        {
            if (!IsEnabledElement)
            {
                return;
            }
            TourRatingViewModel tourRatingViewModel = new TourRatingViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRatingViewModel));
            navigate.Execute(null);
        }
        private void UploadImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";
            openFileDialog.Multiselect = true;
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
                _selectedFiles = openFileDialog.FileNames.ToList();
        }
        private void CopyImages()
        {
            if (_selectedFiles != null && _selectedFiles.Count > 0)
            {
                string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                        "Resources", "Images");
                Directory.CreateDirectory(destinationFolder);

                foreach (string file in _selectedFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string destinationFile = Path.Combine(destinationFolder, fileName);
                    File.Copy(file, destinationFile, true);

                    string relativePath = Path.Combine("Resources", "Images", fileName);
                    _pictureURLs.Add(relativePath);
                }
            }
        }
    }
}
