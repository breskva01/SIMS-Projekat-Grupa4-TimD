using ControlzEx.Standard;
using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InitialProject.Domain.Models
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public Forum Forum { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime PostTime { get; set; }
        public bool CredentialAuthor { get; set; }
        public bool IsOwner { get; set; }
        public bool IsGuest { get; set; }
        public bool VerifiedOwner => CredentialAuthor && Author is Owner;
        public bool VerifiedGuest => CredentialAuthor && Author is Guest1;
        public int ReportCount { get; set; }

        public Comment(Forum forum, string text, User author, DateTime postTime, bool credentialAuthor, bool isOwner, bool isGuest, int reportCount = 0)
        {
            Forum = forum;
            Text = text;
            Author = author;
            PostTime = postTime;
            CredentialAuthor = credentialAuthor;
            IsOwner = isOwner;
            IsGuest = isGuest;
            ReportCount = reportCount;
        }

        public Comment()
        {
            Forum = new Forum();
            Author = new User();
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Forum.Id = int.Parse(values[1]);
            Text = values[2].Replace("\\n", Environment.NewLine);
            Author.Id = int.Parse(values[3]);
            PostTime = DateTime.ParseExact(values[4], "dd.MM.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
            CredentialAuthor = bool.Parse(values[5]);
            IsOwner = bool.Parse(values[6]);
            IsGuest = bool.Parse(values[7]);
            ReportCount = int.Parse(values[8]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            { 
                Id.ToString(),
                Forum.Id.ToString(),
                Text.Replace(Environment.NewLine, "\\n"),
                Author.Id.ToString(),
                PostTime.ToString("dd.MM.yyyy. HH:mm:ss"),
                CredentialAuthor.ToString(),
                IsOwner.ToString(),
                IsGuest.ToString(),
                ReportCount.ToString()
            };
            return csvValues;
        }
    }
}
