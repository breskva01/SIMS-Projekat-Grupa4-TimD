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

namespace InitialProject.WPF.ViewModels.Guest1
{
    public class AccommodationRatingViewModel : ViewModelBase
    {
        public UnratedAccommodationsViewModel UnratedAccommodationsViewModel { get; set; }
        public AccommodationRatingViewModel(NavigationStore navigationStore, User user)
        {
            UnratedAccommodationsViewModel = new UnratedAccommodationsViewModel(navigationStore, user);
        }

    }
}
