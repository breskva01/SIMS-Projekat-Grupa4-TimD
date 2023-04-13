using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Models;

namespace InitialProject.WPF.ViewModels
{
    public class TourViewModel : ViewModelBase
    {
        private readonly Tour _tour;
        public string PictureURL => _tour.PictureURL;
        public string Name => _tour.Name;
        public string Country => _tour.Location?.Country;
        public string City => _tour.Location?.City;
        public string Duration => _tour.Duration.ToString();
        public string Language => _tour.Language.ToString();
        public string CurrentNumberOfGuests => _tour.CurrentNumberOfGuests.ToString();
        public string Description => _tour.Description;
        public string AvailableSpots => (_tour.MaximumGuests - _tour.CurrentNumberOfGuests).ToString();

        public TourViewModel(Tour tour)
        {
            _tour = tour;
        }

        
    }
}
