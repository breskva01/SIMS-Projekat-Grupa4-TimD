using InitialProject.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public void FromCSV(string[] values)
        {
           Id = Convert.ToInt32(values[0]);
           Country = values[1];
           City = values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Country,
                City
            };
            return csvValues;
        }

    }
}
