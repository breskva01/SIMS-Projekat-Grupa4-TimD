using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSlot() { }
        public TimeSlot(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
