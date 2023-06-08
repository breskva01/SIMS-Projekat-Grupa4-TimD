﻿using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class MyVouchersViewModel : ViewModelBase
    {
        public ObservableCollection<Voucher> Vouchers { get; set; }
        private Guest2 _user;
        private readonly NavigationStore _navigationStore;
        private readonly VoucherService _voucherService;

        public ICommand MenuCommand { get; }
        public ICommand FreeVoucherProgressCommand { get; }

        public MyVouchersViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = (Guest2)user;
            _voucherService = new VoucherService();

            List<Voucher> vouchers = new List<Voucher>();
            List<int> voucherIds = _user.VouchersIds;
            foreach (int voucherId in voucherIds)
            {
                vouchers.Add(new Voucher(_voucherService.GetById(voucherId)));
            }
            Vouchers = new ObservableCollection<Voucher>(_voucherService.FilterUnexpired(vouchers));

            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            FreeVoucherProgressCommand = new ExecuteMethodCommand(ShowFreeVoucherProgressView);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowFreeVoucherProgressView()
        {
            FreeVoucherProgressViewModel freeVoucherProgressViewModel = new FreeVoucherProgressViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, freeVoucherProgressViewModel));
            navigate.Execute(null);
        }
    }
}
