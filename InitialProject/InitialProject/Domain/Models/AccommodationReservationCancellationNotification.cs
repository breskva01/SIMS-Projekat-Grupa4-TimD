using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class AccommodationReservationCancellationNotification : ISerializable
    {
        public int ReservationId { get; set; }
        public int OwnerId { get; set; }

        public AccommodationReservationCancellationNotification() { }

        public AccommodationReservationCancellationNotification(int reservationId, int ownerId)
        {
            ReservationId = reservationId;
            OwnerId = ownerId;
        }

        public void FromCSV(string[] values)
        {
            ReservationId = int.Parse(values[0]);
            OwnerId = int.Parse(values[1]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                ReservationId.ToString(),
                OwnerId.ToString()
            };
            return csvValues;
        }
    }

}
