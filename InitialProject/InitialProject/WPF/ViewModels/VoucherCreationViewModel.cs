using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class VoucherCreationViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        private List<User> _guests;

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

        public ICommand BackNavigateCommand =>
           new NavigateCommand(new NavigationService(_navigationStore, GoBack()));

        public ICommand CreateVoucherCommand { get; set; }

        public VoucherCreationViewModel(NavigationStore navigationStore, User user, List<User> guests)
        {
            _navigationStore = navigationStore;
            _user = user;
            _guests = guests;

            _voucherService = new VoucherService();
            _userService = new UserService();

            DateTime dateTime = DateTime.Now;
            _expiration = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateVoucherCommand = new ExecuteMethodCommand(CreateVoucher);
        }

        private void CreateVoucher()
        {
            _expiration = _expiration.AddYears(1);
            Voucher voucher = _voucherService.CreateVoucher(Name, _expiration);
            foreach (User guest in _guests)
            {
                ((Guest2)guest).VouchersIds.Add(voucher.Id);
                _userService.Update(guest);
            }

            BackNavigateCommand.Execute(null);
        }

        private AllToursViewModel GoBack()
        {
            return new AllToursViewModel(_navigationStore, _user);
        }
    }
}
