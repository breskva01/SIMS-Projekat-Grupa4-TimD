using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Location() { }
        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), City, Country
            };
            return csvValues;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Location other = (Location)obj;
            return City == other.City && Country == other.Country;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(City, Country);
        }

        public static bool operator ==(Location location1, Location location2)
        {
            if (ReferenceEquals(location1, location2))
                return true;
            if (location1 is null || location2 is null)
                return false;
            return location1.Equals(location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !(location1 == location2);
        }
    }
}
