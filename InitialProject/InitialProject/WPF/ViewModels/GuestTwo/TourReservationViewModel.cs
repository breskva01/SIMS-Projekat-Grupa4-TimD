using CommunityToolkit.Mvvm.Input;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourReservationViewModel : ViewModelBase
    {
        private Guest2 _user;
        public ObservableCollection<Voucher> Vouchers { get; set; }
        public Tour SelectedTour { get; set; }
        public string AvailableSpots { get; set; }
        private readonly NavigationStore _navigationStore;
        private readonly TourReservationService _tourReservationService;
        private readonly VoucherService _voucherService;

        private Voucher _selectedVoucher;
        public Voucher SelectedVoucher
        {
            get { return _selectedVoucher; }
            set
            {
                _selectedVoucher = value;
                OnPropertyChanged(nameof(SelectedVoucher));
            }
        }

        private int _numberOfGuests;
        public int NumberOfGuests
        {
            get { return _numberOfGuests; }
            set
            {
                if (value > 0 && value <= (SelectedTour.MaximumGuests - SelectedTour.CurrentNumberOfGuests))
                {
                    _numberOfGuests = value;
                    OnPropertyChanged(nameof(NumberOfGuests));
                }

            }
        }
        public ICommand IncreaseNumberCommand => new RelayCommand(() => NumberOfGuests++);
        public ICommand DecreaseNumberCommand => new RelayCommand(() => NumberOfGuests--);
        public ICommand ReserveCommand { get; }
        public ICommand UseVoucherCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }

        public TourReservationViewModel(NavigationStore navigationStore, User user, Tour tour)
        {
            _navigationStore = navigationStore;
            _tourReservationService = new TourReservationService();
            _voucherService = new VoucherService();
            _user = (Guest2)user;
            SelectedTour = tour;
            AvailableSpots = (SelectedTour.MaximumGuests - SelectedTour.CurrentNumberOfGuests).ToString();
            NumberOfGuests = 1;

            List<Voucher> vouchers = new List<Voucher>();
            List<int> voucherIds = _user.VouchersIds;
            foreach (int voucherId in voucherIds)
            {
                vouchers.Add(new Voucher(_voucherService.GetById(voucherId)));
            }
            vouchers = _voucherService.FilterUnused(vouchers);
            Vouchers = new ObservableCollection<Voucher>(_voucherService.FilterGuideVouchers(vouchers, SelectedTour.GuideId));

            ReserveCommand = new ExecuteMethodCommand(MakeReservation);
            CancelCommand = new ExecuteMethodCommand(ShowTourBrowserView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationBrowserView);
        }

        private void MakeReservation()
        {
            if(SelectedVoucher != null)
            {
                TourReservation tourReservationWithoutVoucher = _tourReservationService.CreateReservation(SelectedTour.Id, _user.Id, _numberOfGuests, true);
                SelectedVoucher.State = VoucherState.Used;
                _voucherService.Update(SelectedVoucher);
                ShowTourBrowserView();
                return;
            }
            TourReservation tourReservationWithVoucher = _tourReservationService.CreateReservation(SelectedTour.Id, _user.Id, _numberOfGuests, false);
            ShowTourBrowserView();
        }

        private void ShowTourBrowserView()
        {
            NewTourBrowserViewModel newTourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, newTourBrowserViewModel));
            navigate.Execute(null);
        }

        private void ShowGuest2MenuView()
        {
            Guest2MenuViewModel guest2MenuViewModel = new Guest2MenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2MenuViewModel));
            navigate.Execute(null);
        }

        private void ShowNotificationBrowserView()
        {
            NotificationBrowserViewModel notificationBrowserViewModel = new NotificationBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, notificationBrowserViewModel));
            navigate.Execute(null);
        }

    }
}
