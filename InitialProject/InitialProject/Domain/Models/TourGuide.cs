using InitialProject.Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class TourGuide : User
    {
        public override string[] ToCSV()
        {
            var csvValues = base.ToCSV();
            csvValues = ArrayHandler.AddObjectToArrayStart(csvValues, "TourGuide");
            return csvValues;
        }
    }
}
