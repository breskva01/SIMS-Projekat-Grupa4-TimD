using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Domain.Models
{
    public enum ForumStatus { Open, Closed }
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public User Initiator { get; set; }
        public ForumStatus Status { get; set; }
        public List<Comment> Comments { get; set; }
        public Location Location { get; set; }
        public string Topic { get; set; }
        public bool VeryUseful { get; set; }
        public DateTime LatestCommentPostTime => Comments.LastOrDefault().PostTime;
        public int CommentCount => Comments.Count;
        public int OwnerComments { get; set; }
        public int GuestComments { get; set; }

        public Forum(User initiator, ForumStatus status, Location location, string topic, bool veryUseful)
        {
            Initiator = initiator;
            Status = status;
            Comments = new List<Comment>();
            Location = location;
            Topic = topic;
            VeryUseful = veryUseful;
        }

        public Forum()
        {
            Initiator = new User();
            Comments = new List<Comment>();
            Location = new Location();
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Initiator.Id = int.Parse(values[1]);
            Status = (ForumStatus)Enum.Parse(typeof(ForumStatus), values[2]);
            Location.Id = int.Parse(values[3]);
            Topic = values[4];
            VeryUseful = bool.Parse(values[5]);
            OwnerComments = int.Parse(values[6]);
            GuestComments = int.Parse(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Initiator.Id.ToString(),
                Status.ToString(),
                Location.Id.ToString(),
                Topic,
                VeryUseful.ToString(),
                OwnerComments.ToString(),
                GuestComments.ToString()
            };
            return csvValues;
        }
    }
}
