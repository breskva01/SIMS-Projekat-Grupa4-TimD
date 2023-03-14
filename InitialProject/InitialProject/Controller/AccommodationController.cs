using InitialProject.Model;
using InitialProject.Model.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Controller
{
    public class AccommodationController
    {
        private readonly AccommodationDAO _accommodationDAO;

        public AccommodationController()
        {
            _accommodationDAO = new AccommodationDAO();
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationDAO.GetAll();
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            return _accommodationDAO.GetFiltered(keyWords, type, guestNumber, numberOfDays);
        }
        public void RegisterAccommodation(string name, string country, string city, AccommodationType type, int maximumGuests,
            int minimumDays, int minimumCancelationNotice, string pictureURL)
        {
            _accommodationDAO.Add(name, country, city, type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURL);
        }
    }
}
