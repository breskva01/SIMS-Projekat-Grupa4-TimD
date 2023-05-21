using InitialProject.Application.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public bool SpendABonusPoint()
        {
            if (BonusPoints == 0)
                return false;
            BonusPoints--;
            return true;
        }
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
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, SuperGuestActivationDate.ToString("dd/MM/yyyy"));
            return csvValues;
        }
        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
            SuperGuest = bool.Parse(values[9]);
            BonusPoints = int.Parse(values[10]);
            SuperGuestActivationDate = DateOnly.ParseExact(values[11], "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
