using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class VoucherBrowserViewModel : ViewModelBase
    {
        public ObservableCollection<Voucher> Vouchers { get; set; }
        private Guest2 _user;
        public Tour SelectedTour;
        public int NumberOfGuests;
        private readonly NavigationStore _navigationStore;
        private readonly TourReservationService _tourReservationService;
        private readonly VoucherService _voucherService;

        public ICommand ReserveCommand { get; }
        public ICommand CancelCommand { get; }

        public VoucherBrowserViewModel(NavigationStore navigationStore, User user, Tour tour, int numberOfGuests)
        {
            _navigationStore = navigationStore;
            _user = (Guest2)user;
            SelectedTour = tour;
            NumberOfGuests = numberOfGuests;
            _voucherService = new VoucherService();
            _tourReservationService = new TourReservationService();

            List<Voucher> vouchers = new List<Voucher>();
            List<int> voucherIds = _user.VouchersIds;
            foreach (int voucherId in voucherIds)
            {
                vouchers.Add(new Voucher(_voucherService.GetById(voucherId)));
            }
            Vouchers = new ObservableCollection<Voucher>(_voucherService.FilterUnused(vouchers));

            ReserveCommand = new UseVoucherCommand(MakeReservation);
            CancelCommand = new ExecuteMethodCommand(ShowTourReservationView);
        }

        private void ShowTourBrowserView()
        {
            NewTourBrowserViewModel newTourBrowserViewModel = new NewTourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, newTourBrowserViewModel));
            navigate.Execute(null);
        }

        private void MakeReservation(Voucher voucher)
        {
            TourReservation tourReservation = _tourReservationService.CreateReservation(SelectedTour.Id, _user.Id, NumberOfGuests, true);
            voucher.State = VoucherState.Used;
            _voucherService.Update(voucher);

            ShowTourBrowserView();
        }

        private void ShowTourReservationView()
        {
            TourReservationViewModel tourReservationViewModel = new TourReservationViewModel(_navigationStore, _user, SelectedTour);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourReservationViewModel));
            navigate.Execute(null);
        }
    }
}
