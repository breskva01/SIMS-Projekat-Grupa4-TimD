using InitialProject.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Guest1 : User
    {
        public bool SuperGuest { get; set; }

        public override string[] ToCSV()
        {
            var csvValues = base.ToCSV();
            csvValues = ArrayHandler.AddObjectToArrayStart(csvValues, "Guest1");
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, SuperGuest.ToString());
            return csvValues;
        }
        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
            SuperGuest = Convert.ToBoolean(values[9]);
        }
    }
}
