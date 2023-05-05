using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
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
        private readonly IVoucherRepository _repository;

        public VoucherService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryInjector.Get<IVoucherRepository>();
        }

        public List<Voucher> GetAll()
        {
            return _repository.GetAll();
        }
        public Voucher GetById(int voucherId)
        {
            return _repository.GetById(voucherId);
        }
        public Voucher CreateVoucher(string name, DateOnly expirationDate)
        {
            Voucher voucher = new Voucher();
            voucher.Name = name;
            voucher.ExpirationDate = expirationDate;
            return _repository.Save(voucher);
        }
        public Voucher Update(Voucher voucher)
        {
            return _repository.Update(voucher);
        }
        
        public List<Voucher> FilterUnused(List<Voucher> vouchers)
        {
            return _repository.FilterUnused(vouchers);
        }
        public List<Voucher> FilterUnexpired(List<Voucher> vouchers)
        {
            return _repository.FilterUnexpired(vouchers);
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

    }
}
