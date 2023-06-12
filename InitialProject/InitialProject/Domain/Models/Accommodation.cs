using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum RenovationStatus
    {
        Available, Renovating
    }
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
        public Location Location { get; set; }
        public string Address { get; set; }
        public AccommodationType Type { get; set; }
        public int MaximumGuests { get; set; }
        public int MinimumDays { get; set; }
        public int MinimumCancelationNotice { get; set; }
        public List<string> PictureURLs { get; set; }
        public string MainPictureURL => PictureURLs[0];
        public Owner Owner { get; set; }
        public RenovationStatus Status { get; set; }
        public bool RecentlyRenovated { get; set; }
        public Accommodation() 
        {
            Owner = new Owner();
            Location = new Location();
            PictureURLs = new List<string>();
        }
        public Accommodation(int id, string name, Location location, string address, AccommodationType type, int maximumGuests, int minimumDays,
                             int minimumCancelationNotice, List<string> pictureURLs, Owner owner, RenovationStatus status, bool recentlyRenovated)
        {
            Id = id;
            Name = name;
            Location = location;
            Address = address;
            Type = type;
            MaximumGuests = maximumGuests;
            MinimumDays = minimumDays;
            MinimumCancelationNotice = minimumCancelationNotice >= 1 ? minimumCancelationNotice : 1;
            PictureURLs = pictureURLs;
            Owner = owner;
            Status = status;
            RecentlyRenovated = recentlyRenovated;
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
            string[] splitKeyWords = keyWords.ToLower().Split(" ");
            foreach (string keyWord in splitKeyWords)
            {
                if (! (Name.ToLower().Contains(keyWord) ||
                       Location.City.ToLower().Contains(keyWord) ||
                       Location.Country.ToLower().Contains(keyWord) ))
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
            Location.Id = Convert.ToInt32(values[2]);
            Address = values[3];
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[4]);
            MaximumGuests = Convert.ToInt32(values[5]);
            MinimumDays = Convert.ToInt32(values[6]);
            MinimumCancelationNotice = Convert.ToInt32(values[7]);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"Resources\Images\";
            string fullPath = Path.Combine(baseDirectory, relativePath);
            string[] pictureURLs = values[8].Split(';');
            foreach (string pictureURL in pictureURLs) 
            {
                string imagePath = Path.Combine(fullPath, pictureURL);
                PictureURLs.Add(imagePath);
            }
            Owner.Id = Convert.ToInt32(values[9]);
            Status = (RenovationStatus)Enum.Parse(typeof(RenovationStatus), values[10]);
            RecentlyRenovated = bool.Parse(values[11]);
        }

        public string[] ToCSV()
        {
            string[] pictureNames = PictureURLs.Select(url => Path.GetFileName(url)).ToArray();
            string pictureURLs = string.Join(";", pictureNames);

            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Location.Id.ToString(),
                Address,
                Type.ToString(),
                MaximumGuests.ToString(),
                MinimumDays.ToString(),
                MinimumCancelationNotice.ToString(),
                pictureURLs,
                Owner.Id.ToString(),
                Status.ToString(),
                RecentlyRenovated.ToString(),
            };
            return csvValues;
        }
    }
}
