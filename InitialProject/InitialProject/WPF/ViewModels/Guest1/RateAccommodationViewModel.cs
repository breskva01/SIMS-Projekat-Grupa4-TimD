using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class RateAccommodationViewModel : ViewModelBase
    {
        public AccommodationReservation Reservation { get; set; }
        private readonly AccommodationRatingService _ratingService;
        private readonly NavigationStore _navigationStore;
        public ICommand RateReservationCommand { get; }
        public ICommand NavigateRatingsCommand { get; }
        public ICommand UploadImagesCommand { get; }
        public int Location { get; set; }
        public int Hygiene { get; set; }
        public int Pleasantness { get; set; }
        public int Fairness { get; set; }
        public int Parking { get; set; }
        public string Comment { get; set; }
        private List<string> _pictureURLs;
        private List<string> _selectedFiles;
        public RateAccommodationViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            _navigationStore = navigationStore;
            Reservation = reservation;
            _pictureURLs = new List<string>();
            _ratingService = new AccommodationRatingService();
            RateReservationCommand = new ExecuteMethodCommand(SubmitRating);
            NavigateRatingsCommand = new ExecuteMethodCommand(NavigateRatings);
            UploadImagesCommand = new ExecuteMethodCommand(UploadImages);
        }
        public void SubmitRating()
        {
            MessageBoxResult result = MessageBox.Show
                ("Da li ste sigurni da želite da ocenite rezervaciju?", "Potvrda ocene",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CopyImages();
                _ratingService.Save(Reservation, Location, Hygiene, Pleasantness, Fairness,
                              Parking, Comment, _pictureURLs);
                NavigateRatings();
            }      
        }
        private void NavigateRatings()
        {
            var contentViewModel = new AccommodationRatingViewModel(_navigationStore, Reservation.Guest);
            var navigationBarViewModel = new NavigationBarViewModel(_navigationStore, Reservation.Guest);
            var layoutViewModel = new LayoutViewModel(navigationBarViewModel, contentViewModel);
            var navigateCommand = new NavigateCommand(new NavigationService(_navigationStore, layoutViewModel));
            navigateCommand.Execute(null);
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
