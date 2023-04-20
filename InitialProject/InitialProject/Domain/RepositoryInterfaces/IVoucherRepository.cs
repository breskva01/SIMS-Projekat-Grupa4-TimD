using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        public Voucher Update(Voucher voucher);
        public Voucher Save(Voucher voucher);
        public int NextId();
        public void Delete(Voucher voucher);
        public Voucher GetById(int voucherId);
        public List<Voucher> FilterUnexpired(List<Voucher> vouchers);
        public List<Voucher> FilterUnused(List<Voucher> vouchers);
        public bool IsExpired(Voucher voucher);
        public bool MatchesUnusedFilter(Voucher voucher);
    }
}
