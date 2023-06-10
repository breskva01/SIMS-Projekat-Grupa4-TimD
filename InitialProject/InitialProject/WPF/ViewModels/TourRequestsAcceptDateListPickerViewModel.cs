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
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsAcceptDateListPickerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourRequest SelectedRequest;

        public TourRequestsAcceptDateListPickerView View;

        private List<string> _avaiableDates;
        public List<string> AvaiableDates
        {
            get { return _avaiableDates; }
            set
            {
                if (_avaiableDates != value)
                {
                    _avaiableDates = value;
                    OnPropertyChanged(nameof(AvaiableDates));
                }
            }
        }
        private ObservableCollection<DateTime> _dates;
        public ObservableCollection<DateTime> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                OnPropertyChanged(nameof(Dates));
            }
        }

        private List<Tour> tours {  get; set; }
        private TourService _tourService;
        private ObservableCollection<DateTime> _busyDates;

        public ICommand ChooseDateCommand { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (value != _selectedDate)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourRequestsAcceptDateListPickerViewModel(NavigationStore navigationStore, User user, TourRequest SelectedTourRequest, TourRequestsAcceptDateListPickerView view)
        {
            _navigationStore = navigationStore;
            _user = user;
            SelectedRequest = SelectedTourRequest;

            DateTime start = SelectedRequest.EarliestDate;
            DateTime end = SelectedRequest.LatestDate;

            _tourService = new TourService();

            tours = new List<Tour>(_tourService.GetAll());

            _busyDates = new ObservableCollection<DateTime>();
            foreach(Tour tour in tours)
            {
                if(tour.GuideId == user.Id)
                {
                    _busyDates.Add(tour.Start);
                }
            }

            Dates = new ObservableCollection<DateTime>();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                Dates.Add(date.Date);
            }

            foreach(DateTime dateTimeBusy in _busyDates)
            {
                Dates.Remove(dateTimeBusy.Date);
            }

            View = view;

            ChooseDateCommand = new ExecuteMethodCommand(ChooseDate);

        }
        private void ChooseDate()
        {
            View.Close();

            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user, SelectedRequest, SelectedDate.ToString());
            NavigateCommand navigate = new NavigateCommand(new Application.Services.NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
    }
}
