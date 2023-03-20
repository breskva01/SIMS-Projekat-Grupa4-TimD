using InitialProject.Model.DAO;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Controller
{
    public class AccommodationReservationController
    {
        private readonly AccommodationReservationDAO _reservationDAO;

        public AccommodationReservationController()
        {
            _reservationDAO = new AccommodationReservationDAO();
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservationDAO.GetAll();
        }
        public List<AccommodationReservation> FindAvailable(DateTime beginDateTime, DateTime endDateTime, int days, Accommodation accommodation, User guest)
        {
            DateOnly beginDate = DateOnly.FromDateTime(beginDateTime);
            DateOnly endDate = DateOnly.FromDateTime(endDateTime);
            return _reservationDAO.FindAvailable(beginDate, endDate, days, accommodation, guest);
        }
        public List<AccommodationReservation> FindCompletedReservations()
        {
            return _reservationDAO.FindCompletedReservations();
        }
    }
}
