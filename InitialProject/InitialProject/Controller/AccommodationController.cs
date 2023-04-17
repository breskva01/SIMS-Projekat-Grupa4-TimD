using InitialProject.Domain.Models;
using InitialProject.Domain.Models.DAO;
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
        public List<Accommodation> SortByName(List<Accommodation> accommodations)
        {
            return _accommodationDAO.SortByName(accommodations);
        }
        public List<Accommodation> SortByLocation(List<Accommodation> accommodations)
        {
            return _accommodationDAO.SortByLocation(accommodations);
        }
        public List<Accommodation> SortByMaxGuestNumber(List<Accommodation> accommodations)
        {
            return _accommodationDAO.SortByMaxGuestNumber(accommodations);
        }
        public List<Accommodation> SortByMinDaysNumber(List<Accommodation> accommodations)
        {
            return _accommodationDAO.SortByMinDaysNumber(accommodations);
        }
        public void RegisterAccommodation(string name, string country, string city, string address, AccommodationType type, int maximumGuests,
            int minimumDays, int minimumCancelationNotice, string pictureURL, User user, int ownerId)
        {
            _accommodationDAO.Add(name, country, city, address, type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURL, user, ownerId);
        }
    }
}
