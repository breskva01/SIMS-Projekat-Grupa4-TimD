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

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class RateTourViewModel : ViewModelBase
    {
        private User _user;
        private Tour _tour;
        private readonly TourReservationService _tourReservationService;
        private readonly TourRatingService _tourRatingService;
        private readonly NavigationStore _navigationStore;

        public int KnowledgeRating { get; set; }
        public int LanguageRating { get; set; }
        public int InterestingRating { get; set; }
        public int InformativeRating { get; set; }
        public int ContentRating { get; set; }
        public string Comment { get; set; }
        private List<string> _pictureURLs;
        private List<string> _selectedFiles;

        public ICommand BackCommand { get; }
        public ICommand RateCommand { get; }
        public ICommand UploadImagesCommand { get; }

        public RateTourViewModel(NavigationStore navigationStore, User user, Tour tour) {
            _navigationStore = navigationStore;
            _user = user;
            _tour = tour;
            _tourReservationService = new TourReservationService();
            _tourRatingService = new TourRatingService();
            _pictureURLs = new List<string>();

            BackCommand = new ExecuteMethodCommand(ShowTourRatingView);
            RateCommand = new ExecuteMethodCommand(SubmitRating);
            UploadImagesCommand = new ExecuteMethodCommand(UploadImages);
        }
        public void SubmitRating()
        {
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
        private void ShowTourRatingView()
        {
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
