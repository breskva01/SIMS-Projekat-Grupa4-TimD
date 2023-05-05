using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class GuestRating : ISerializable
    {
        public int OwnerId { get; set; }
        public int GuestId { get; set; }
        public int Hygiene { get; set; }
        public int RespectsRules { get; set; }
        public int Communication { get; set; }
        public int Timeliness { get; set; }
        public int NoiseLevel { get; set; }
        public int OverallExperience { get; set; }
        public string Comment { get; set; }

        public GuestRating() { }

        public GuestRating(int ownerId, int guestId, int hygiene, int respectsRules, int communication, int timeliness, int noiseLevel, int overallExperince, string comment)
        {
            OwnerId = ownerId;
            GuestId = guestId;
            Hygiene = hygiene;
            RespectsRules = respectsRules;
            Communication = communication;
            Timeliness = timeliness;
            NoiseLevel = noiseLevel;
            OverallExperience= overallExperince;
            Comment = comment;
        }

        public void FromCSV(string[] values)
        {
            OwnerId = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            Hygiene = int.Parse(values[2]);
            RespectsRules = int.Parse(values[3]);
            Communication= int.Parse(values[4]);
            Timeliness= int.Parse(values[5]);
            NoiseLevel  = int.Parse(values[6]);
            OverallExperience= int.Parse(values[7]);
            Comment = values[4];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
                {
                OwnerId.ToString(),
                GuestId.ToString(),
                Hygiene.ToString(),
                RespectsRules.ToString(),
                Communication.ToString(),
                Timeliness.ToString(),
                NoiseLevel.ToString(),
                OverallExperience.ToString(),
                Comment
            };
            return csvValues;
        }
    }
}
