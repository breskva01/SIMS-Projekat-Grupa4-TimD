using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class LocationPopularity
    {
        public Location Location { get; set; }
        public int Reservations { get; set; }
        public double OccupancyRate { get; set; }

        public LocationPopularity() { }
        public LocationPopularity(Location location, int reservations, double occupancyRate) 
        {
            Location = location;
            Reservations= reservations;
            OccupancyRate= occupancyRate;
        }
        
    }
}
