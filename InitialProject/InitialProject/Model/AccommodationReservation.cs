using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int NumberOfDays { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public DateOnly LastNotification { get; set; }
        public AccommodationReservation() { }
        public AccommodationReservation(Accommodation accommodation, User guest, int numberOfDays, DateOnly checkInDate, DateOnly checkOutDate)
        {
            AccommodationId = accommodation.Id;
            Accommodation = accommodation;
            GuestId = guest.Id;
            Guest = guest;
            NumberOfDays = numberOfDays;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
        }
        public bool Overlap(DateOnly checkIn, DateOnly checkOut)
        {
            return CheckInDate < checkOut && checkIn < CheckOutDate;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            NumberOfDays = int.Parse(values[3]);
            CheckInDate = DateOnly.Parse(values[4]);
            CheckOutDate = DateOnly.Parse(values[5]);
            LastNotification = DateOnly.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), GuestId.ToString(),
                                    NumberOfDays.ToString(), CheckInDate.ToString(), CheckOutDate.ToString(),
                                    LastNotification.ToString() };
            return csvValues;
        }
    }
}
