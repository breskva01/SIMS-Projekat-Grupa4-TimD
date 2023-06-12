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
        public bool VerifiedOwner => CredentialAuthor && Author is Owner;
        public bool VerifiedGuest => CredentialAuthor && Author is Guest1;
        public int ReportCount { get; set; }
        public List<int> ReportIds { get; set; }

        public Comment(Forum forum, string text, User author, DateTime postTime, bool credentialAuthor, int reportCount = 0)
        {
            Forum = forum;
            Text = text;
            Author = author;
            PostTime = postTime;
            CredentialAuthor = credentialAuthor;
            ReportCount = reportCount;
            ReportIds = new List<int>();
        }

        public Comment()
        {
            Forum = new Forum();
            Author = new User();
            ReportIds= new List<int>();
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Forum.Id = int.Parse(values[1]);
            Text = values[2];
            Author.Id = int.Parse(values[3]);
            PostTime = DateTime.ParseExact(values[4], "dd.MM.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
            CredentialAuthor = bool.Parse(values[5]);
            ReportCount = int.Parse(values[6]);
            string reportIds = values[7];
            string[] splitReportIds = reportIds.Split(',');
            splitReportIds = splitReportIds.SkipLast(1).ToArray();
            ReportIds = new List<int>();
            foreach (string reportId in splitReportIds)
            {
                ReportIds.Add(Convert.ToInt32(reportId));
            }

        }

        public string[] ToCSV()
        {
            string reportIds = "";
            foreach (int report in ReportIds)
            {
                reportIds += report.ToString() + ",";
            }
            string[] csvValues =
            { 
                Id.ToString(),
                Forum.Id.ToString(),
                Text,
                Author.Id.ToString(),
                PostTime.ToString("dd.MM.yyyy. HH:mm:ss"),
                CredentialAuthor.ToString(),
                ReportCount.ToString(),
                reportIds
            };
            return csvValues;
        }
    }
}
