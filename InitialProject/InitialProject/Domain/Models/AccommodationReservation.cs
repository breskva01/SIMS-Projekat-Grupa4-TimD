using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum AccommodationReservationStatus { Finished, Active, Cancelled }
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public User Guest { get; set; }
        public int NumberOfDays { get; set; }
        public int GuestNumber { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public DateOnly LastNotification { get; set; }
        public bool IsGuestRated { get; set; }
        public bool IsOwnerRated { get; set; }
        public AccommodationReservationStatus Status { get; set; }
        public AccommodationReservation() 
        {
            Guest = new User();
            Accommodation = new Accommodation();
        }
        public AccommodationReservation(Accommodation accommodation, User guest, int numberOfDays, DateOnly checkIn,
            DateOnly checkOut)
        {
            Accommodation = accommodation;
            Guest = guest;
            NumberOfDays = numberOfDays;
            CheckIn = checkIn;
            CheckOut = checkOut;
            IsGuestRated = false;
            IsOwnerRated = false;
            Status = AccommodationReservationStatus.Active;
        }
        public bool Overlaps(DateOnly checkIn, DateOnly checkOut)
        {
            return CheckIn < checkOut && checkIn < CheckOut;
        }
        public bool CanBeCancelled()
        {
            DateOnly todaysDate = DateOnly.FromDateTime(DateTime.Now);
            TimeSpan difference = CheckIn.ToDateTime(TimeOnly.MinValue) -
                                  todaysDate.ToDateTime(TimeOnly.MinValue);
            int differenceInDays = (int)difference.TotalDays;
            return differenceInDays >= Accommodation.MinimumCancelationNotice;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Accommodation.Id = int.Parse(values[1]);
            Guest.Id = int.Parse(values[2]);
            NumberOfDays = int.Parse(values[3]);
            GuestNumber = int.Parse(values[4]);
            CheckIn = DateOnly.ParseExact(values[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            CheckOut = DateOnly.ParseExact(values[6], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            LastNotification = DateOnly.ParseExact(values[7], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            IsGuestRated = bool.Parse(values[8]);
            IsOwnerRated = bool.Parse(values[9]);
            Status = (AccommodationReservationStatus)Enum.Parse(typeof(AccommodationReservationStatus), values[10]);
            if (CheckOut <= DateOnly.FromDateTime(DateTime.Now) && Status == AccommodationReservationStatus.Active)
            {
                Status = AccommodationReservationStatus.Finished;
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Accommodation.Id.ToString(), Guest.Id.ToString(),
                                    NumberOfDays.ToString(), GuestNumber.ToString(), CheckIn.ToString("dd/MM/yyyy"), CheckOut.ToString("dd/MM/yyyy"),
                                    LastNotification.ToString("dd/MM/yyyy"), IsGuestRated.ToString(), IsOwnerRated.ToString(), Status.ToString()};
            return csvValues;
        }
    }
}
