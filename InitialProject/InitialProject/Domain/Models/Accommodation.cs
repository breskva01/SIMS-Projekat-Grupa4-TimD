using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum AccommodationType { Apartment, House, Cottage, Everything }
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public AccommodationType Type { get; set; }
        public int MaximumGuests { get; set; }
        public int MinimumDays { get; set; }
        public int MinimumCancelationNotice { get; set; }
        public string PictureURL { get; set; }
        public User Owner { get; set; }
        public int OwnerId { get; set; }
        public Accommodation() { }
        public Accommodation(int id, string name, string country, string city, string address, AccommodationType type, int maximumGuests, int minimumDays,
                             int minimumCancelationNotice, string pictureURL, User owner, int ownerId)
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
            Address = address;
            Type = type;
            MaximumGuests = maximumGuests;
            MinimumDays = minimumDays;
            MinimumCancelationNotice = minimumCancelationNotice;
            PictureURL = pictureURL;
            Owner = owner;
            OwnerId = ownerId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            City = values[2];
            Country = values[3];
            Address = values[4];
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[5]);
            MaximumGuests = Convert.ToInt32(values[6]);
            MinimumDays = Convert.ToInt32(values[7]);
            MinimumCancelationNotice = Convert.ToInt32(values[8]);
            PictureURL = values[9];
            OwnerId = Convert.ToInt32(values[10]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
                { Id.ToString(),
                  Name,
                  City,
                  Country,
                  Address,
                  Type.ToString(),
                  MaximumGuests.ToString(),
                  MinimumDays.ToString(),
                  MinimumCancelationNotice.ToString(),
                  PictureURL, OwnerId.ToString() };
            return csvValues;
        }
    }
}
