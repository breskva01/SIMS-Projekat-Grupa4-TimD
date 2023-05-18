using InitialProject.Application.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public enum RequestStatus
    {
        OnHold,
        Invalid,
        Accepted
    }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public GuideLanguage Language { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime EarliestDate { get; set; }
        public DateTime LatestDate { get; set; } 

        public TourRequest()
        {
            Location = new Location();
            Description = string.Empty;
            Language = GuideLanguage.All;
            NumberOfGuests = 0;
            EarliestDate = DateTime.MinValue;
            LatestDate = DateTime.MaxValue;
        }

        public TourRequest(int id, Location location, string description, GuideLanguage language, int numberOfGuests, DateTime earliestDate, DateTime latestDate)
        {
            Id = id;
            Location = location;
            Description = description;
            Language = language;
            NumberOfGuests = numberOfGuests;
            EarliestDate = earliestDate;
            LatestDate = latestDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location.Id = Convert.ToInt32(values[1]);
            Description = values[2];
            Language = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), values[3]);
            NumberOfGuests = Convert.ToInt32(values[4]);
            EarliestDate = DateTime.ParseExact(values[5], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
            LatestDate = DateTime.ParseExact(values[6], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Location.Id.ToString(),
                Description,
                Language.ToString(),
                NumberOfGuests.ToString(),
                EarliestDate.ToString("dd.MM.yyyy. HH:mm:ss"),
                LatestDate.ToString("dd.MM.yyyy. HH:mm:ss"),
            };
            return csvValues;
        }
    }

}
