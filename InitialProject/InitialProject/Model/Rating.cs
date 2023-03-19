using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    class Rating: ISerializable
    {
        public int Hygiene { get; set; }
        public int RespectsRules { get; set; }
        public string Comment { get; set; }

        public Rating() { }

        public Rating(int hygiene, int respectsRules, string comment)
        {
            Hygiene = hygiene;
            RespectsRules = respectsRules;
            Comment = comment;
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }
    }
}
