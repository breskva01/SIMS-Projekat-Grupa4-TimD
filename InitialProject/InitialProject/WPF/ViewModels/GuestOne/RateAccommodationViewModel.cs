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
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
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
        private bool _renovatingNeeded;
        public bool RenovatingNeeded
        {
            get => _renovatingNeeded;
            set
            {
                if (_renovatingNeeded != value)
                {
                    _renovatingNeeded = value;
                    OnPropertyChanged();
                }
            }
        }
        public string RenovationComment { get; set; }
        public int RenovationUrgency { get; set; }
        public string ToolTipText { get; } =
            "Nivo 1 - bilo bi lepo renovirati neke sitnice, ali sve funkcioniše kako treba i bez toga\r\n" +
            "Nivo 2 - male zamerke na smeštaj koje kada bi se uklonile bi ga učinile savršenim\r\n" +
            "Nivo 3 - nekoliko stvari koje su baš zasmetale bi trebalo renovirati\r\n" +
            "Nivo 4 - ima dosta loših stvari i renoviranje je stvarno neophodno\r\n" +
            "Nivo 5 - smeštaj je u jako lošem stanju i ne vredi ga uopšte iznajmljivati ukoliko se ne renovira\r\n";
        public RateAccommodationViewModel(NavigationStore navigationStore, AccommodationReservation reservation)
        {
            _navigationStore = navigationStore;
            Reservation = reservation;
            RenovatingNeeded = false;
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
                _ratingService.Save(Reservation, Location, Hygiene, Pleasantness, Fairness, Parking, Comment,
                                    _pictureURLs, RenovatingNeeded, RenovationComment, RenovationUrgency);
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
