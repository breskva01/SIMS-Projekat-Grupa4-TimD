using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Attendance : ISerializable
    {
        public int Id { get; set; }
        public User Guest { get; set; }
        public int GuestId { get; set; }
        public KeyPoint KeyPoint { get; set; }
        public int KeyPointId { get; set; }
        public DateTime Arrival { get; set; }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            KeyPointId = Convert.ToInt32(values[2]);
            Arrival = Convert.ToDateTime(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                KeyPointId.ToString(),
                Arrival.ToString()
            };
            return csvValues;
        }
    }
}
