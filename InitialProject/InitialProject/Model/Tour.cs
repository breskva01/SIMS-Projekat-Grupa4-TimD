using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xaml.Schema;
using System.Xml.Linq;
using InitialProject.Serializer;
using InitialProject.Model;

namespace InitialProject.Model
{
    public enum GuideLanguage
    {
        Serbian,
        English
    }
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; }
        public GuideLanguage Language { get; set; }

        public int MaximumGuests { get; set; }
        
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public string PictureURL { get; set; }
        public int CurrentNumberOfGuests { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }

        public List<int> KeyPointIds { get; set; }
        public Tour() 
        {
            Name = string.Empty;
            City = new City();
            Description = string.Empty;
            PictureURL = string.Empty;
            KeyPoints = new List<KeyPoint>();
        }

        public Tour(int id, string name, int cityId, string description, GuideLanguage language, int maximumGuests, DateTime start, int duration, string pictureURL, int currentNumberOfGuests, List<KeyPoint> ky)
        {
            Id = id;
            Name = name;
            CityId = cityId;
            Description = description;
            Language = language;
            MaximumGuests = maximumGuests;
            Start = start;
            Duration = duration;
            PictureURL = pictureURL;
            CurrentNumberOfGuests = currentNumberOfGuests;
            KeyPoints = ky;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            CityId = Convert.ToInt32(values[2]);
            /*City.Name = values[2];
            City.Country = values[3];*/ 
            Description = values[3];

            Language = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), values[4]);
            MaximumGuests = Convert.ToInt32(values[5]);
            Start = DateTime.Parse(values[6]);
            Duration = Convert.ToInt32(values[7]);
            PictureURL = values[8];
            // maybe change the name of field to NumberOfGuests
            CurrentNumberOfGuests = Convert.ToInt32(values[9]);
            string keyPoints = values[10];
            string[] splitKeyPoints = keyPoints.Split(',');
            splitKeyPoints = splitKeyPoints.SkipLast(1).ToArray();
            KeyPointIds = new List<int>();
            foreach(string keyPoint in splitKeyPoints)
            {
               KeyPointIds.Add(Convert.ToInt32(keyPoint));
            }
            foreach(int ky in KeyPointIds)
            {
                Console.WriteLine(ky);
            }

        }
        public string[] ToCSV()
        {
            string keyPoints = "";
            foreach (KeyPoint ky in KeyPoints)
            {
                keyPoints += ky.Id.ToString() + ",";
            }
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                CityId.ToString(),
                Description,
                Language.ToString(),
                MaximumGuests.ToString(),
                Start.ToString(), 
                Duration.ToString(),  
                PictureURL.ToString(),
                CurrentNumberOfGuests.ToString(),
                keyPoints
            };
            return csvValues;
        }
    }
}
