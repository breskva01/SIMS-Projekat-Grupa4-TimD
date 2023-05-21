using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestTwo
{
    public class TourRequestStats3ViewModel : ViewModelBase
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

        private string _generalNumberOfGuests;
        public string GeneralNumberOfGuests
        {
            get { return _generalNumberOfGuests; }
            set
            {
                _generalNumberOfGuests = value;
                OnPropertyChanged(nameof(GeneralNumberOfGuests));

            }
        }


        private string _yearNumberOfGuests;
        public string YearNumberOfGuests
        {
            get { return _yearNumberOfGuests; }
            set
            {
                _yearNumberOfGuests = value;
                OnPropertyChanged(nameof(YearNumberOfGuests));

            }
        }

        public ICommand BackCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand NotificationCommand { get; }

        public TourRequestStats3ViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            _availableYears = new List<string>();


            _tourRequestService = new TourRequestService();
            GeneralNumberOfGuests = _tourRequestService.GetGeneralNumberOfGuests().ToString();
            YearNumberOfGuests = string.Empty;

            AvailableYears = _tourRequestService.GetAvailableYears();

            BackCommand = new ExecuteMethodCommand(ShowTourRequestStats2View);
            MenuCommand = new ExecuteMethodCommand(ShowGuest2MenuView);
            NotificationCommand = new ExecuteMethodCommand(ShowNotificationBrowserView);

        }

        
        private void ShowTourRequestStats2View()
        {
            TourRequestStats2ViewModel tourRequestStats2ViewModel = new TourRequestStats2ViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourRequestStats2ViewModel));
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
            YearNumberOfGuests = _tourRequestService.GetYearNumberOfGuests(_selectedYear).ToString();
        }
    }
}
