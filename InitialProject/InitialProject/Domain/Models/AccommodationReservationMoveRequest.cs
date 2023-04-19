using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum ReservationMoveRequestStatus { Accepted, Declined, Pending }
    public class AccommodationReservationMoveRequest : ISerializable
    {
        public AccommodationReservation Reservation { get; set; }
        public int ReservationId { get; set; }
        public DateOnly RequestedCheckIn { get; set; }
        public DateOnly RequestedCheckOut { get; set; }
        public string Comment { get; set; }
        public ReservationMoveRequestStatus Status { get; set; }
        public bool GuestNotified { get; set; }

        public AccommodationReservationMoveRequest() { }

        public AccommodationReservationMoveRequest(AccommodationReservation reservation,
            DateOnly requestedCheckIn, DateOnly requestedCheckOut)
        {
            Reservation = reservation;
            ReservationId = reservation.Id;
            RequestedCheckIn = requestedCheckIn;
            RequestedCheckOut = requestedCheckOut;
            Status = ReservationMoveRequestStatus.Pending;
            GuestNotified = false;
        }

        public void FromCSV(string[] values)
        {
            ReservationId = int.Parse(values[0]);
            RequestedCheckIn = DateOnly.ParseExact(values[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            RequestedCheckOut = DateOnly.ParseExact(values[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Comment = values[3];
            Status = (ReservationMoveRequestStatus)Enum.Parse(typeof(ReservationMoveRequestStatus), values[4]);
            GuestNotified = bool.Parse(values[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                ReservationId.ToString(),
                RequestedCheckIn.ToString("dd/MM/yyyy"),
                RequestedCheckOut.ToString("dd/MM/yyyy"),
                Comment,
                Status.ToString(),
                GuestNotified.ToString()
            };
            return csvValues;
        }
    }

}
