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
        public int RatingId { get; set; }
        public bool UsedVoucher { get; set; }

        public TourReservation() {
            Presence = Presence.Absent;
            ArrivedAtKeyPoint = 0;
            RatingId = -1; //Guest wasn't present on the tour || Tour hasn't finnished
            UsedVoucher = false;
        }
        public TourReservation(int id, int tourId, Tour tour, int guestId, User guest, int numberOfGuests, Presence presence, int arrivedAtKeyPoint, int ratingId, bool usedVoucher)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            GuestId = guestId;
            Guest = guest;
            NumberOfGuests = numberOfGuests;
            Presence = presence;
            ArrivedAtKeyPoint = arrivedAtKeyPoint;
            RatingId = ratingId;
            UsedVoucher = usedVoucher;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
            Presence = (Presence)Enum.Parse(typeof(Presence), values[4]);
            ArrivedAtKeyPoint = Convert.ToInt32(values[5]);
            RatingId = Convert.ToInt32(values[6]);
            UsedVoucher = Convert.ToBoolean(values[7]);
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
                ArrivedAtKeyPoint.ToString(),
                RatingId.ToString(),
                UsedVoucher.ToString()
            };
            return csvValues;
        }
    }
}
