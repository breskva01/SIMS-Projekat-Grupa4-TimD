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
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourRequestStats1ViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;
        private TourRequestService _tourRequestService;
        
        private List<string> _availableYears;
        public List<string> AvailableYears
        {
            get { return _availableYears; }
            set
            {
                _availableYears = value;
                OnPropertyChanged(nameof(AvailableYears));
            }
        }

        private string _selectedYear;
        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                YearsSelectionChanged();

            }
        }

        private string _generalApproved;
        public string GeneralApproved
        {
            get { return _generalApproved; }
            set
            {
                _generalApproved = value;
                OnPropertyChanged(nameof(GeneralApproved));

            }
        }

        private string _generalDeclined;
        public string GeneralDeclined
        {
            get { return _generalDeclined; }
            set
            {
                _generalDeclined = value;
                OnPropertyChanged(nameof(GeneralDeclined));

            }
        }

        private string _forYearApproved;
        public string ForYearApproved
        {
            get { return _forYearApproved; }
            set
            {
                _forYearApproved = value;
                OnPropertyChanged(nameof(ForYearApproved));

            }
        }

        private string _forYearDeclined;
        public string ForYearDeclined
        {
            get { return _forYearDeclined; }
            set
            {
                _forYearDeclined = value;
                OnPropertyChanged(nameof(ForYearDeclined));

            }
        }

        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }

        public TourRequestStats1ViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _availableYears = new List<string>();


            _tourRequestService = new TourRequestService();
            GeneralApproved = _tourRequestService.GetApprovedGeneral().ToString();
            GeneralApproved += " %";
            GeneralDeclined = (100 - _tourRequestService.GetApprovedGeneral()).ToString();
            GeneralDeclined += " %";
            ForYearApproved = string.Empty;
            ForYearDeclined = string.Empty;

            AvailableYears = _tourRequestService.GetAvailableYears();

            NextCommand = new ExecuteMethodCommand(ShowTourRequestStats2View);
            BackCommand = new ExecuteMethodCommand(ShowGuest2RequestMenuView);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationBrowserView);

        }

        private void ShowGuest2RequestMenuView()
        {
            Guest2RequestMenuViewModel guest2RequestMenuViewModel = new Guest2RequestMenuViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, guest2RequestMenuViewModel));
            navigate.Execute(null);
        }
        private void ShowTourRequestStats2View()
        {
            TourRequestStats3ViewModel tourRequestStats3ViewModel = new TourRequestStats3ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStats3ViewModel));
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

        private void YearsSelectionChanged()
        {
            ForYearApproved = _tourRequestService.GetApprovedForYear(_selectedYear).ToString();
            ForYearApproved += " %";
            ForYearDeclined = (100 - _tourRequestService.GetApprovedForYear(_selectedYear)).ToString();
            ForYearDeclined += " %";
        }

    }
}
