using InitialProject.Domain.Models;
using InitialProject.Domain.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Controller
{
    public class GuestRatingController
    {
        private readonly GuestRatingDAO _guestRatingDAO;

        public GuestRatingController()
        {
            _guestRatingDAO = new GuestRatingDAO();
        }

        public List<GuestRating> GetAll()
        {
            return _guestRatingDAO.GetAll();
        }
        public GuestRating RateGuest(int ownerId, int guestId, int hygiene, int respectsRules, int communication, int timeliness, int noiseLevel, int overallExperience, string comment)
        {
            return _guestRatingDAO.Add(ownerId, guestId, hygiene, respectsRules, communication, timeliness, noiseLevel, overallExperience, comment);
        }
    }
}
