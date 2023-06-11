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
using System.Windows;
using InitialProject.Application.Observer;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class AccommodationRatingViewModel : ViewModelBase
    {
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value != _selectedTab)
                {
                    _selectedTab = value;
                    if (_selectedTab > 1)
                        _selectedTab = 0;
                    OnPropertyChanged();
                }
            }
        }
        public UnratedAccommodationsViewModel UnratedAccommodationsViewModel { get; set; }
        public ReceivedRatingsViewModel ReceivedRatingsViewModel { get; set; }
        public AccommodationRatingViewModel(NavigationStore navigationStore, Guest1 user, int selectedTab = 0)
        {
            UnratedAccommodationsViewModel = new UnratedAccommodationsViewModel(navigationStore, user);
            ReceivedRatingsViewModel = new ReceivedRatingsViewModel(navigationStore, user);
            SelectedTab = selectedTab;
        }

    }
}
