using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Application.Serializer;

namespace InitialProject.Domain.Models
{
    public enum VoucherState
    {
        Unused,
        Used,
        Expired
    }
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public VoucherState State { get; set; }
        public int GuideId { get; set; }

        public Voucher() { 
            Name = string.Empty;
            State = VoucherState.Unused;
        }

        /*
        public Voucher(int id, string name, DateOnly expiration)
        {
            Id = id;
            Name = name;
            ExpirationDate = expiration;
            State = VoucherState.Unused;
        }
        */
        public Voucher(Voucher voucher)
        {
            Id = voucher.Id;
            Name = voucher.Name;
            ExpirationDate = voucher.ExpirationDate;
            State = voucher.State;
            GuideId = voucher.GuideId;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            ExpirationDate = DateOnly.Parse(values[2]);
            State = (VoucherState)Enum.Parse(typeof(VoucherState),values[3]);
            GuideId = Convert.ToInt32(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                ExpirationDate.ToString(),
                State.ToString(),
                GuideId.ToString()
            };
            return csvValues;
        }
    }
}
