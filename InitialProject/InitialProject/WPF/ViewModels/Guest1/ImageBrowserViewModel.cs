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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class ImageBrowserViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        private readonly Accommodation _accommodation;
        private readonly List<string> _imageUrls;
        private int _currentIndex;
        private string _currentImageUrl;

        public string CurrentImageUrl
        {
            get => _currentImageUrl;
            set
            {
                if (_currentImageUrl != value)
                {
                    _currentImageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NavigateReservationFormCommand { get; }
        public ICommand PreviousImageCommand { get; }
        public ICommand NextImageCommand { get; }

        public ImageBrowserViewModel(NavigationStore navigationStore, User user,
            Accommodation accommodation, string imageUrl)
        {
            _navigationStore = navigationStore;
            _user = user;
            _accommodation = accommodation;
            _imageUrls = accommodation.PictureURLs;
            _currentIndex = _imageUrls.IndexOf(imageUrl);
            CurrentImageUrl = imageUrl;

            NavigateReservationFormCommand = new ExecuteMethodCommand(NavigateReservationForm);
            PreviousImageCommand = new ExecuteMethodCommand(PreviousImage);
            NextImageCommand = new ExecuteMethodCommand(NextImage);
        }

        private void NextImage()
        {
            _currentIndex++;
            if (_currentIndex >= _imageUrls.Count)
            {
                _currentIndex = 0;
            }
            CurrentImageUrl = _imageUrls[_currentIndex];
        }

        private void PreviousImage()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _imageUrls.Count - 1;
            }
            CurrentImageUrl = _imageUrls[_currentIndex];
        }

        private void NavigateReservationForm()
        {
            var viewModel = new AccommodationReservationViewModel(_navigationStore, _user, _accommodation);
            new NavigationService(_navigationStore, viewModel).Navigate();
        }
    }

}
