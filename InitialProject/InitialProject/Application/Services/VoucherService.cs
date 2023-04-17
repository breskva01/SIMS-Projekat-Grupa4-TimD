using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.Application.Services
{
    public class VoucherService
    {
        private readonly List<IObserver> _observers;
        private readonly VoucherRepository _repository;
        public VoucherService()
        {
            _observers = new List<IObserver>();
            _repository = new VoucherRepository();
        }
        public List<Voucher> GetAll()
        {
            return _repository.GetAll();
        }
        public Voucher Update(Voucher voucher)
        {
            return _repository.Update(voucher);
        }
        public Voucher GetById(int voucherId)
        {
            return _repository.GetById(voucherId);
        }

        public List<Voucher> FilterUnused(List<Voucher> vouchers)
        {
            return _repository.FilterUnused(vouchers);
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }
        public Voucher CreateVoucher(string name, DateOnly expirationDate)
        {
            Voucher voucher = new Voucher();
            voucher.Name = name;
            voucher.ExpirationDate = expirationDate;
            return _repository.Save(voucher);
        }
    }
}
