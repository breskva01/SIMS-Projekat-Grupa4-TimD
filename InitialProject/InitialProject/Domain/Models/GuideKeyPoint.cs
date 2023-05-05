using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class GuideKeyPoint 
    {
        public KeyPoint KeyPoint { get; set; }
        public int KeyPointId { get; set; }
        public int GuideId { get; set; }
        public bool IsReached { get; set; }

        public GuideKeyPoint() { }  


    }
}
