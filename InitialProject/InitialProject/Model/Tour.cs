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
    class Tour
    {
        public int Id { get; set; }
        //public Guide Guide { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int MaximumGuests { get; set; }
        //kljucne tacke ?
        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public string PictureURL { get; set; }
        public int Guests { get; set; }
        public Tour() { }

        // fale kljucne tacke
        public Tour(int id, /*Guide guide*/ string name, Location location, string description, Language language, int maximumGuests, DateTime start, int duration, string pictureURL, int guests)
        {
            Id = id;
            //Guide = guide;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaximumGuests = maximumGuests;
            Start = start;
            Duration = duration;
            PictureURL = pictureURL;
            Guests = guests;
        }
    }
}
