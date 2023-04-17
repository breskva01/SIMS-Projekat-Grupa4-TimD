using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Voucher() { 
            Name = string.Empty;
            State = VoucherState.Unused;
            //ExpirationDate = (DateOnly.FromDateTime(DateTime.UtcNow)).AddMonths(1);
        }

        public Voucher(int id, string name, DateOnly expiration)
        {
            Id = id;
            Name = name;
            ExpirationDate = expiration;
            State = VoucherState.Unused;
        }
        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }
    }
}
