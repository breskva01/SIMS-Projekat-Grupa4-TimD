using InitialProject.Application.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum Presence
    {
        Absent,
        Pending,
        Present
    }
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int NumberOfGuests { get; set; }
        public Presence Presence { get; set; }
        public int ArrivedAtKeyPoint { get; set; }

        public TourReservation() {
            Presence = Presence.Absent;
            ArrivedAtKeyPoint = 0;
        }
        public TourReservation(int id, int tourId, Tour tour, int guestId, User guest, int numberOfGuests, Presence presence, int arrivedAtKeyPoint)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            GuestId = guestId;
            Guest = guest;
            NumberOfGuests = numberOfGuests;
            Presence = presence;
            ArrivedAtKeyPoint = arrivedAtKeyPoint;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
            Presence = (Presence)Enum.Parse(typeof(Presence), values[4]);
            ArrivedAtKeyPoint = Convert.ToInt32(values[5]);

        }
        
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                TourId.ToString(),
                GuestId.ToString(),
                NumberOfGuests.ToString(),
                Presence.ToString(),
                ArrivedAtKeyPoint.ToString()
            };
            return csvValues;
        }
    }
}
