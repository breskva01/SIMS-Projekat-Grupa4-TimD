using InitialProject.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Controller
{
    public class LocationController
    {
        private readonly LocationDAO _locationDAO;
        public LocationController() 
        {
            _locationDAO = new LocationDAO();
        }
        public void Create(String Country, String City)
        {
            
        }
    }
}
