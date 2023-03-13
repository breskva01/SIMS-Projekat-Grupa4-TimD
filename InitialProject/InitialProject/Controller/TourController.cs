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
        public void CreateTour() 
        {

        }
    }
}
