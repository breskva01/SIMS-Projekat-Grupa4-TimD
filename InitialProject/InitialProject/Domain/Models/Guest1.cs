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
        public int BonusPoints { get; set; }
        public DateOnly SuperGuestActivationDate { get; set; }
        public void ActivateSuperGuest()
        {
            SuperGuest = true;
            SuperGuestActivationDate = DateOnly.FromDateTime(DateTime.Now);
            BonusPoints = 5;
        }
        public void CancelSuperGuest()
        {
            SuperGuest = false;
            BonusPoints = 0;
        }
        public bool HasSuperGuestExpired()
        {
            DateOnly todaysDate = DateOnly.FromDateTime(DateTime.Now);
            TimeSpan difference = todaysDate.ToDateTime(TimeOnly.MinValue) - 
                                  SuperGuestActivationDate.ToDateTime(TimeOnly.MinValue);
            int differenceInDays = (int)difference.TotalDays;
            return differenceInDays > 365;
        }
        public override string[] ToCSV()
        {
            var csvValues = base.ToCSV();
            csvValues = ArrayHandler.AddObjectToArrayStart(csvValues, "Guest1");
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, SuperGuest.ToString());
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, BonusPoints.ToString());
            return csvValues;
        }
        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
            SuperGuest = bool.Parse(values[9]);
            BonusPoints = int.Parse(values[10]);
        }
    }
}
