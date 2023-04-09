using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Model.DAO;

namespace InitialProject.Controller
{
    public class TourController
    {
        private readonly TourDAO _tourDAO;

        public TourController()
        {
            _tourDAO = new TourDAO();
        }

        public List<Tour> GetAll()
        {
            return _tourDAO.GetAll();
        }
        public Tour CreateTour(string Name, Location Location, string Description,GuideLanguage Language, 
            int MaximumGuests, DateTime Start,int Duration, string PictureUrl, List<KeyPoint> ky, List<int> kyIds) 
        {
            int LocationId = Location.Id;
            Tour Tour = new Tour();
            Tour.Name = Name;
            Tour.Location = Location;
            Tour.LocationId = LocationId;
            Tour.Description = Description;
            Tour.Language = Language;
            Tour.MaximumGuests = MaximumGuests;
            Tour.Start = Start;
            Tour.Duration = Duration;
            Tour.PictureURL = PictureUrl;
            Tour.CurrentNumberOfGuests = 0;
            Tour.KeyPoints = ky;
            Tour.KeyPointIds = kyIds;  
            return _tourDAO.Save(Tour);
        }
        public Tour Update(Tour tour)
        {
            return _tourDAO.Update(tour);
        }

        public List<Tour> GetFiltered(string country, string city, int duration, GuideLanguage language, int NumberOfGuests)
        {
            return _tourDAO.GetFiltered(country, city, duration, language, NumberOfGuests);
        }

        public List<Tour> SortByName(List<Tour> tours)
        {
            return _tourDAO.SortByName(tours);
        }
        public List<Tour> SortByLocation(List<Tour> tours)
        {
            return _tourDAO.SortByLocation(tours);
        }
        public List<Tour> SortByDuration(List<Tour> tours)
        {
            return _tourDAO.SortByDuration(tours);
        }
        public List<Tour> SortByLanguage(List<Tour> tours)
        {
            return _tourDAO.SortByLanguage(tours);
        }
    }
}
