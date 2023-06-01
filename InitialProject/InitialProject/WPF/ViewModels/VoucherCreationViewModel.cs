using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Application.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using InitialProject.WPF.NewViews;

namespace InitialProject.WPF.ViewModels
{
    public class VoucherCreationViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private List<User> _guests;
        private List<Voucher> _vouchers;

        public int NumberOfYears;

        private VoucherService _voucherService;
        private UserService _userService;

        private DateOnly _expiration;


        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /*public ICommand BackNavigateCommand =>
           new NavigateCommand(new NavigationService(_navigationStore, GoBack()));*/

        public VoucherCreationView View;
        public bool Resign;

        public ICommand CreateVoucherCommand { get; set; }

        public VoucherCreationViewModel(NavigationStore navigationStore, User user, List<User> guests, int years, VoucherCreationView view, bool resign)
        {
            _navigationStore = navigationStore;
            _user = user;
            _guests = guests;

            _voucherService = new VoucherService();
            _userService = new UserService();

            DateTime dateTime = DateTime.Now;
            _expiration = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);

            NumberOfYears = years;
            View = view;
            _vouchers = new List<Voucher>(_voucherService.GetAll());
            Resign= resign;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateVoucherCommand = new ExecuteMethodCommand(CreateVoucher);
            
        }
        private void SignOut()
        {
            SignInViewModel signInViewModel = new SignInViewModel(_navigationStore);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, signInViewModel));

            navigate.Execute(null);
        }
        private void CreateVoucher()
        {
            _expiration = _expiration.AddYears(NumberOfYears);
            Voucher voucher = _voucherService.CreateVoucher(Name, _expiration, _user);
            
            if(Resign == true)
            {

                foreach (User guest in _guests)
                {
                    voucher.GuideId = -1;
                    _voucherService.Update(voucher);
                    ((Guest2)guest).VouchersIds.Add(voucher.Id);
                    _userService.Update(guest);
                    for(int i = 0; i < ((Guest2)guest).VouchersIds.Count() - 1; i++)
                    {
                        Voucher v = _voucherService.GetById(((Guest2)guest).VouchersIds[i]);
                        if (v.GuideId == _user.Id)
                        {
                            v.GuideId = -1;
                            _voucherService.Update(v);
                        }
                    }
                    
                    View.Close();
                    SignOut();
                    return;
                }
            }
            else
            {
                foreach (User guest in _guests)
                {
                    ((Guest2)guest).VouchersIds.Add(voucher.Id);
                    _userService.Update(guest);
                    View.Close();
                    return;
                }
            }
            
        }

    }
}
/*
moram da dobijem sve goste koji ce dobiti vaucer
gost koji dobija vaucer je 




 */