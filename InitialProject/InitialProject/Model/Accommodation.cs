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
    public enum AccommodationType { Apartment, House, Cottage, Everything}
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public string Address { get; set; }
        public AccommodationType Type { get; set; }
        public int MaximumGuests { get; set; }
        public int MinimumDays { get; set; }
        public int MinimumCancelationNotice { get; set; }
        //verovatno treba promeniti i implementirati drugacije
        public string PictureURL { get; set; }
        public User Owner { get; set; }
        public int OwnerId { get; set; }
        public Accommodation() { }
        public Accommodation(int id, string name, Location location, int locationId, string address, AccommodationType type, int maximumGuests, int minimumDays, 
                             int minimumCancelationNotice, string pictureURL, User owner, int ownerId)
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = locationId;
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
            LocationId = Convert.ToInt32(values[2]);
            Address = values[3]; 
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[4]);
            MaximumGuests = Convert.ToInt32(values[5]);
            MinimumDays = Convert.ToInt32(values[6]);
            MinimumCancelationNotice = Convert.ToInt32(values[7]);
            PictureURL = values[8];
            OwnerId = Convert.ToInt32(values[9]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
                { Id.ToString(), 
                  Name, 
                  LocationId.ToString(), 
                  Address, Type.ToString(), 
                  MaximumGuests.ToString(), 
                  MinimumDays.ToString(), 
                  MinimumCancelationNotice.ToString(), 
                  PictureURL, OwnerId.ToString() };
            return csvValues;
        }
    }
}
