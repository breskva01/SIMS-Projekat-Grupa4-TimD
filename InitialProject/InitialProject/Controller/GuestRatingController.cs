using InitialProject.Domain.Model;
using InitialProject.Domain.Model.DAO;
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
        public GuestRating RateGuest(int ownerId, int guestId, int hygiene, int respectsRules, string comment)
        {
            return _guestRatingDAO.Add(ownerId, guestId, hygiene, respectsRules, comment);
        }
    }
}
