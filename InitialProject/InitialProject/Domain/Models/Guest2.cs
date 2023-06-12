using InitialProject.Application.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Guest2 : User
    {
        public List<int> VouchersIds { get; set; }
        public int FreeVoucherProgress { get; set; }
        public DateTime FreeVoucherProgressLimit { get; set; }
        public override string[] ToCSV()
        {
            string voucherIds = "";
            foreach (int vId in VouchersIds)
            {
                voucherIds += vId.ToString() + ",";
            }

            var csvValues = base.ToCSV();
            csvValues = ArrayHandler.AddObjectToArrayStart(csvValues, "Guest2");
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, voucherIds);
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, FreeVoucherProgress.ToString());
            csvValues = ArrayHandler.AddObjectToArrayEnd(csvValues, FreeVoucherProgressLimit.ToString("dd.MM.yyyy. HH:mm:ss"));
            return csvValues;
        }
        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
            string vouchers = values[9];
            string[] splitVouchers = vouchers.Split(',');
            splitVouchers = splitVouchers.SkipLast(1).ToArray();
            VouchersIds = new List<int>();
            foreach (string voucherId in splitVouchers)
            {
                VouchersIds.Add(Convert.ToInt32(voucherId));
            }
            FreeVoucherProgress = Convert.ToInt32(values[10]);
            FreeVoucherProgressLimit = DateTime.ParseExact(values[11], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);

        }
    }
}
