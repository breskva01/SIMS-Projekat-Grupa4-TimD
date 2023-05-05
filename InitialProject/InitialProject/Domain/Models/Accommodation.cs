using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum AccommodationType
    {
        [Display(Name = "Kuća")]
        House,
        [Display(Name = "Apartman")]
        Apartment,
        [Display(Name = "Koliba")]
        Cottage,
        [Display(Name = "Sve")]
        Everything
    }
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
        public Accommodation() 
        {
            Owner = new User();
        }
        public Accommodation(int id, string name, string country, string city, string address, AccommodationType type, int maximumGuests, int minimumDays,
                             int minimumCancelationNotice, string pictureURL, User owner)
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
        }
        public bool MatchesFilters(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            bool keyWordsMatch = Contains(keyWords);
            bool typeMatch = Type == type || type == AccommodationType.Everything;
            bool maximumGestsMatch = MaximumGuests >= guestNumber;
            bool minimumDaysMatch = MinimumDays <= numberOfDays;
            return keyWordsMatch && typeMatch && maximumGestsMatch && minimumDaysMatch;
        }
        private bool Contains(string keyWords)
        {
            if (string.IsNullOrEmpty(keyWords))
                return true;
            string[] splitKeyWords = keyWords.Split(" ");
            foreach (string keyWord in splitKeyWords)
            {
                if (!(Name.ToLower().Contains(keyWord.ToLower()) ||
                    City.ToLower().Contains(keyWord.ToLower()) ||
                    Country.ToLower().Contains(keyWord.ToLower())))
                {
                    return false;
                }
            }
            return true;
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
            Owner.Id = Convert.ToInt32(values[10]);
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
                  PictureURL, Owner.Id.ToString() };
            return csvValues;
        }
    }
}
