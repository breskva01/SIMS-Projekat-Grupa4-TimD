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
    //Everything only used for filter purposes, no accommodation should be assigned this value
    public enum AccommodationType { Apartment, House, Cottage, Everything}
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public AccommodationType Type { get; set; }
        public int MaximumGuests { get; set; }
        public int MinimumDays { get; set; }
        public int MinimumCancelationNotice { get; set; }
        //verovatno treba promeniti i implementirati drugacije
        public string PictureURL { get; set; }
        public Accommodation() { }
        public Accommodation(int id, string name, string country, string city, AccommodationType type, int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            Type = type;
            MaximumGuests = maximumGuests;
            MinimumDays = minimumDays;
            MinimumCancelationNotice = minimumCancelationNotice;
            PictureURL = pictureURL;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Country = values[2];
            City = values[3];
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[4]);
            MaximumGuests = Convert.ToInt32(values[5]);
            MinimumDays = Convert.ToInt32(values[6]);
            MinimumCancelationNotice = Convert.ToInt32(values[7]);
            PictureURL = values[8];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Country, City, Type.ToString(), MaximumGuests.ToString(), MinimumDays.ToString(), MinimumCancelationNotice.ToString(), PictureURL };
            return csvValues;
        }
    }
}
