using InitialProject.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Owner : User
    {
        public double Rating { get; set; }
        public bool SuperOwner { get; set; }
        public override string[] ToCSV()
        {
            var csvValues = base.ToCSV();
            csvValues = ArrayHandler.AddObjectToArrayStart(csvValues, "Owner");
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, Rating.ToString());
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, SuperOwner.ToString());
            return csvValues;
        }
        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
            Rating = Convert.ToDouble(values[9]);
            SuperOwner = Convert.ToBoolean(values[10]);
        }
    }
}
