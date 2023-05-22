using InitialProject.Application.Services;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class ApprovedRequestViewModel : ViewModelBase
    {
        private UserService _userService;
        private TourService _tourService;
        private readonly TourRequest _tourRequest;
        private Tour _tour;
        private User _guide;

        public string RequestId => _tourRequest.Id.ToString();
        public string Country => _tourRequest.Location.Country;
        public string City => _tourRequest.Location.City;
        public string Language => _tourRequest.Language.ToString();
        public string NumberOfGuests => _tourRequest.NumberOfGuests.ToString();
        public string Guide => _guide.FirstName + " " + _guide.LastName;
        public string Date => _tour.Start.ToString("dd-MM-yyyy");

        public ApprovedRequestViewModel(TourRequest tourRequest)
        {
            _tourRequest = tourRequest;

            _userService = new UserService();
            _tourService = new TourService();
            _tour = _tourService.GetById(_tourRequest.TourId);
            _guide = _userService.GetById(_tour.GuideId);
        }
    }
}
