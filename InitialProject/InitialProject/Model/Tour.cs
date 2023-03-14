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
        All,
        Serbian,
        English
    }
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }
        //public int CityId { get; set; }
        public string Description { get; set; }
        public GuideLanguage Language { get; set; }

        public int MaximumGuests { get; set; }
        
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public string PictureURL { get; set; }
        public int CurrentNumberOfGuests { get; set; }
        public Tour() 
        {
            Name = string.Empty;
            City = new City();
            Description = string.Empty;
            PictureURL = string.Empty;
        }

        public Tour(int id, string name, City city, string description, GuideLanguage language, int maximumGuests, DateTime start, int duration, string pictureURL, int currentNumberOfGuests)
        {
            Id = id;
            Name = name;
            City = city;
            Description = description;
            Language = language;
            MaximumGuests = maximumGuests;
            Start = start;
            Duration = duration;
            PictureURL = pictureURL;
            CurrentNumberOfGuests = currentNumberOfGuests;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            //CityId = Convert.ToInt32(values[2]);
            City.Name = values[2];
            City.Country = values[3];
            Description = values[4];
            Language = (GuideLanguage)Enum.Parse(typeof(GuideLanguage), values[5]);
            MaximumGuests = Convert.ToInt32(values[6]);
            Start = DateTime.Parse(values[7]);
            Duration = Convert.ToInt32(values[8]);
            PictureURL = values[9];
            // maybe change the name of field to NumberOfGuests
            CurrentNumberOfGuests = Convert.ToInt32(values[10]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                //CityId.ToString(),
                City.Name,
                City.Country,
                Description,
                Language.ToString(),
                MaximumGuests.ToString(),
                Start.ToString(), 
                Duration.ToString(),  
                PictureURL.ToString(),
                CurrentNumberOfGuests.ToString()
            };
            return csvValues;
        }
    }
}
