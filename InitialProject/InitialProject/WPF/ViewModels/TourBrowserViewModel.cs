using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using InitialProject.Application.Commands;
using InitialProject.Application.Services;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.ViewModels
{
    public class TourBrowserViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TourViewModel> _tours;
        public TourViewModel SelectedTour { get; set; }

        
        public ObservableCollection<TourViewModel> Tours => _tours;

        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand ApplySortCommand { get; }
        public ICommand MakeReservationCommand { get; }
        private readonly NavigationStore _navigationStore;
        private User _user;


        public TourBrowserViewModel(NavigationStore navigationStore, User user)
        {
            _tours = new ObservableCollection<TourViewModel>();
            _navigationStore = navigationStore;
            _user = user;

            //MakeReservationCommand = new NavigateCommand(tourReservationNavigationService);

            //UpdateTours();
        }

        //private void UpdateTours()
        //{
        //    throw new NotImplementedException();
       // }
    }
}
