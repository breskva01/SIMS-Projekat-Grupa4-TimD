using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public DateTime RenovationExpiration { get; set; }
        public AccommodationRenovation() 
        {
            Accommodation = new Accommodation();
        }
        public AccommodationRenovation(int id, Accommodation accommodation, DateTime start, DateTime end, string description, DateTime renovationExpiration)
        {
            Id = id;
            Accommodation = accommodation;
            Start = start;
            End = end;
            Description = description;
            RenovationExpiration = renovationExpiration;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
           {
                Id.ToString(),
                Accommodation.Id.ToString(),
                Start.ToString(),
                End.ToString(),
                Description,
                RenovationExpiration.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Accommodation.Id = int.Parse(values[1]);
            Start = DateTime.Parse(values[2]);
            End = DateTime.Parse(values[3]);
            Description= values[4];
            RenovationExpiration= DateTime.Parse(values[5]);
        }
    }
}
