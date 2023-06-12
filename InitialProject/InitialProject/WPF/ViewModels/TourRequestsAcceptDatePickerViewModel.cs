using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using InitialProject.Application.Services;
using System.Windows;
using System.Collections.ObjectModel;
using LiveCharts.Wpf.Charts.Base;
using System.ComponentModel;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestsAcceptDatePickerViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourRequest SelectedRequest;

        public ICommand ConfirmDateCommand { get; set; }

        private string _selectedDate;
        public string SelectedDate
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

        public TourRequestsAcceptDatePickerView View;



        public TourRequestsAcceptDatePickerViewModel(NavigationStore navigationStore, User user,TourRequest SelectedTourRequest, TourRequestsAcceptDatePickerView view)
        {
            _navigationStore = navigationStore;
            _user = user;
            SelectedRequest = SelectedTourRequest;
            ConfirmDateCommand = new ExecuteMethodCommand(ConfirmDate);

            View = view;
        }
        private void ConfirmDate()
        {
            View.Close();

            TourCreationViewModel viewModel = new TourCreationViewModel(_navigationStore, _user, SelectedRequest, SelectedDate);
            NavigateCommand navigate = new NavigateCommand(new Application.Services.NavigationService(_navigationStore, viewModel));
            navigate.Execute(null);
        }
        public string this[string columnName]
        {
            get
            {
                string? error = null;
                string requiredMessage = "Obavezno polje";
                switch (columnName)
                {
                    case nameof(SelectedDate):
                        
                            if (Convert.ToDateTime(SelectedDate) < SelectedRequest.EarliestDate || Convert.ToDateTime(SelectedDate) > SelectedRequest.LatestDate) error = "NOPE";
                        
                        break;
                    default:
                        break;
                }
                return error;

            }
        }
        public string Error => null;
        public bool IsTourValid
        {
            get
            {
                foreach (var property in new string[]
                {
                    nameof(SelectedDate) })
                {
                    if (this[property] != null) return false;
                }
                return true;
            }

        }
    }
}
