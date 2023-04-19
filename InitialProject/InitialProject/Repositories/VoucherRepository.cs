using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly VoucherFileHandler _voucherFileHandler;
        private List<Voucher> _vouchers;

        public VoucherRepository()
        {
            _voucherFileHandler = new VoucherFileHandler();
            _vouchers = _voucherFileHandler.Load();
        }

        public List<Voucher> GetAll()
        {
            return _voucherFileHandler.Load();
        }
        public Voucher GetById(int voucherId)
        {
            _vouchers = _voucherFileHandler.Load();
            foreach (Voucher voucher in _vouchers)
            {
                if (IsExpired(voucher))
                {
                    voucher.State = VoucherState.Expired;
                    Update(voucher);
                }
            }
            List<Voucher> updatedVouchers = _voucherFileHandler.Load();
            return updatedVouchers.Find(v => v.Id == voucherId);
        }
        public Voucher Update(Voucher voucher)
        {
            _vouchers = _voucherFileHandler.Load();
            Voucher updated = _vouchers.Find(v => v.Id == voucher.Id);
            _vouchers.Remove(updated);
            _vouchers.Add(voucher);
            _voucherFileHandler.Save(_vouchers);
            return voucher;
        }
        public void Delete(Voucher voucher)
        {
            _vouchers = _voucherFileHandler.Load();
            Voucher founded = _vouchers.Find(v => v.Id == voucher.Id);
            _vouchers.Remove(founded);
            _voucherFileHandler.Save(_vouchers);
        }
        public Voucher Save(Voucher voucher)
        {
            voucher.Id = NextId();
            _vouchers = _voucherFileHandler.Load();
            _vouchers.Add(voucher);
            _voucherFileHandler.Save(_vouchers);
            return voucher;
        }
        public int NextId()
        {
            _vouchers = _voucherFileHandler.Load();
            if (_vouchers.Count < 1)
            {
                return 1;
            }
            return _vouchers.Max(t => t.Id) + 1;
        }
        public List<Voucher> FilterUnexpired(List<Voucher> vouchers)
        {
            List<Voucher> filteredVouchers = new List<Voucher>();
            foreach (Voucher voucher in vouchers)
            {
                //CheckVoucherExpiration(voucher);
                if (!IsExpired(voucher))
                {
                    filteredVouchers.Add(voucher);
                }
            }
            return filteredVouchers;
        }
        public List<Voucher> FilterUnused(List<Voucher> vouchers)
        {
            List<Voucher> filteredVouchers = new List<Voucher>();
            foreach (Voucher v in vouchers)
            {
                if (MatchesUnusedFilter(v))
                {
                    filteredVouchers.Add(v);
                }
            }
            return filteredVouchers;
        }
        public bool MatchesUnusedFilter(Voucher voucher)
        {
            return voucher.State == VoucherState.Unused;
        }
        public bool IsExpired(Voucher voucher)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            return today.CompareTo(voucher.ExpirationDate) > 0;
        }
    }
}
