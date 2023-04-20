using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationRating : ISerializable
    {
        public AccommodationReservation Reservation { get; set; }
        public int Location { get; set; }
        public int Hygiene { get; set; }
        public int Pleasantness { get; set; }
        public int Fairness { get; set; }
        public int Parking { get; set; }
        public string Comment { get; set; }
        public List<string> PictureURLs { get; set; }
        public AccommodationRating() 
        {
            Comment = "";
            PictureURLs = new List<string>();
            Reservation = new AccommodationReservation();
        }
        public AccommodationRating(AccommodationReservation reservation, int location, int hygiene, 
            int pleasantness, int fairness, int parking, string comment, List<string> pictureURLs)
        {
            Reservation = reservation;
            Location = location;
            Hygiene = hygiene;
            Pleasantness = pleasantness;
            Fairness = fairness;
            Parking = parking;
            Comment = comment;
            PictureURLs = pictureURLs;
        }
        public void FromCSV(string[] values)
        {
            Reservation.Id = int.Parse(values[0]);
            Location = int.Parse(values[1]);
            Hygiene = int.Parse(values[2]);
            Pleasantness = int.Parse(values[3]);
            Fairness = int.Parse(values[4]);
            Parking = int.Parse(values[5]);
            Comment = values[6];
            PictureURLs = new List<string>(values[7].Split(','));
        }

        public string[] ToCSV()
        {
            string pictureURLs = string.Join(",", PictureURLs);
            string[] csvValues =
            {
                Reservation.Id.ToString(),
                Location.ToString(),
                Hygiene.ToString(),
                Pleasantness.ToString(),
                Fairness.ToString(),
                Parking.ToString(),
                Comment,
                pictureURLs
            };
            return csvValues;
        }
    }
}
