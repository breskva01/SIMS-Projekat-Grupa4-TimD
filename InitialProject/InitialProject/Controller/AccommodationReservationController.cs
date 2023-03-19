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
    }
}
