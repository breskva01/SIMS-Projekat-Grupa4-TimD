using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AverageAccommodationRatings
    {
        public AccommodationReservation Reservation { get; set; }
        public double Location { get; set; }
        public double Hygiene { get; set; }
        public double Pleasantness { get; set; }
        public double Fairness { get; set; }
        public double Parking { get; set; }
        public AverageAccommodationRatings() 
        {
            Reservation = new AccommodationReservation();
        }
        public AverageAccommodationRatings(AccommodationReservation reservation, double location, double hygiene, 
            double pleasantness, double fairness, double parking)
        {
            Reservation = reservation;
            Location = location;
            Hygiene = hygiene;
            Pleasantness = pleasantness;
            Fairness = fairness;
            Parking = parking;
        }
    }
}
