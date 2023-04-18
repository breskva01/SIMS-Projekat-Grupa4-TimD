using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    class VoucherFileHandler
    {
        private const string _vouchersFilePath = "../../../Resources/Data/vouchers.csv";
        private readonly Serializer<Voucher> _serializer;

        public VoucherFileHandler()
        {
            _serializer = new Serializer<Voucher>();
        }
        public List<Voucher> Load()
        {
            return _serializer.FromCSV(_vouchersFilePath);
        }
        public void Save(List<Voucher> vouchers)
        {
            _serializer.ToCSV(_vouchersFilePath, vouchers);
        }
    }
}
