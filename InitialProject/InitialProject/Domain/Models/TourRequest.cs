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
        Approved
    }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public RequestStatus Status { get; set; }

        public GuideLanguage Language { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime EarliestDate { get; set; }
        public DateTime LatestDate { get; set; } 

        public TourRequest()
        {
            Location = new Location();
            Description = string.Empty;
            Status = RequestStatus.OnHold;
            Language = GuideLanguage.All;
            NumberOfGuests = 0;
            EarliestDate = DateTime.MinValue;
            LatestDate = DateTime.MaxValue;
        }

        public TourRequest(int id, int userId, Location location, string description, RequestStatus status, GuideLanguage language, int numberOfGuests, DateTime earliestDate, DateTime latestDate)
        {
            Id = id;
            UserId = userId;
            Location = location;
            Description = description;
            Status = status;
            Language = language;
            NumberOfGuests = numberOfGuests;
            EarliestDate = earliestDate;
            LatestDate = latestDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Location.Id = Convert.ToInt32(values[2]);
            Description = values[3];
            Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), values[4]);
            Language = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), values[5]);
            NumberOfGuests = Convert.ToInt32(values[6]);
            EarliestDate = DateTime.ParseExact(values[7], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
            LatestDate = DateTime.ParseExact(values[8], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Location.Id.ToString(),
                Description,
                Status.ToString(),
                Language.ToString(),
                NumberOfGuests.ToString(),
                EarliestDate.ToString("dd.MM.yyyy. HH:mm:ss"),
                LatestDate.ToString("dd.MM.yyyy. HH:mm:ss"),
            };
            return csvValues;
        }
    }

}
