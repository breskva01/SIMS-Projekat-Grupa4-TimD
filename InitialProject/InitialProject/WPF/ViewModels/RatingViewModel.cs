using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class RatingViewModel : ViewModelBase
    {
        private readonly TourReservation _tourReservation;
        private UserService _userService;
        private KeyPointService _keyPointService;
        private TourRatingService _tourRatingService;
        public TourRating tourRating;
        private User guest;

        public string FirstName => guest.FirstName;
        public string LastName => guest.LastName;
        public string GuideKnowledge => tourRating.GuideKnowledge.ToString();
        public string GuideLanguage => tourRating.GuideLanguage.ToString();
        public string TourInteresting => tourRating.TourInteresting.ToString();
        public string TourInformative => tourRating.TourInformative.ToString();
        public string TourContent => tourRating.TourContent.ToString();
        public string Comment => tourRating.Comment;
        public string Place => (_keyPointService.GetById(_tourReservation.ArrivedAtKeyPoint)).Place;

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));

            }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));

            }
        }




        public RatingViewModel(TourReservation tourReservation)
        {
            _tourReservation = tourReservation;

            _userService = new UserService();
            _keyPointService = new KeyPointService();
            _tourRatingService = new TourRatingService();
            tourRating = _tourRatingService.Get(_tourReservation.RatingId);
            guest = _userService.GetById(_tourReservation.GuestId);
            _isValid = tourRating.IsValid;
            _id = tourRating.Id;
        }
    }
}
