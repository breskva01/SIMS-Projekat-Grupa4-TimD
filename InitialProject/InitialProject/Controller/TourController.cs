using System;
using System.Collections.Generic;
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
        public Tour CreateTour(string Name, City City, string Description,GuideLanguage Language, int MaximumGuests, DateTime Start,int Duration, string PictureUrl, List<KeyPoint> ky) 
        {
            int CityId = City.CityId;
            Tour Tour = new Tour();
            Tour.Name = Name;
            Tour.City = City;
            Tour.CityId = CityId;
            Tour.Description = Description;
            Tour.Language = Language;
            Tour.MaximumGuests = MaximumGuests;
            Tour.Start = Start;
            Tour.Duration = Duration;
            Tour.PictureURL = PictureUrl;
            Tour.CurrentNumberOfGuests = 0;
            Tour.KeyPoints = ky;
            return _tourDAO.Save(Tour);
        }
    }
}
