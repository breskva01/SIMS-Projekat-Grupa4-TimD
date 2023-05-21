using InitialProject.Application.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class Guest2 : User
    {
        public List<int> VouchersIds { get; set; }
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
        }
    }
}
