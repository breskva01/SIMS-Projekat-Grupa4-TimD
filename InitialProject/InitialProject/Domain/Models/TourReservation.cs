using InitialProject.Application.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int NumberOfGuests { get; set; }

        public TourReservation() { }
        public TourReservation(int id, int tourId, Tour tour, int guestId, User guest, int numberOfGuests)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            GuestId = guestId;
            Guest = guest;
            NumberOfGuests = numberOfGuests;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourId.ToString(),
                GuestId.ToString(),
                NumberOfGuests.ToString(),
            };
            return csvValues;
        }
    }
}
