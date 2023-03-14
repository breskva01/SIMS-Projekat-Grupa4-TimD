using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class City : ISerializable
    {   
        
        public string Name { get; set; }
        public string Country { get; set; }
        public City() { }
        public void FromCSV(string[] values)
        {
            Name = values[0];
            Country = values[1];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Name, Country
            };
            return csvValues;
        }
    }
}
