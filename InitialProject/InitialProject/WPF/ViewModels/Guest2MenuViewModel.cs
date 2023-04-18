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

namespace InitialProject.WPF.ViewModels
{
    public class Guest2MenuViewModel : ViewModelBase
    {
        private User _user;
        private readonly NavigationStore _navigationStore;

        public ICommand MyVouchersCommand { get; }
        public ICommand TourBrowserCommand { get; }
        public ICommand TrackingCommand { get; }
        public Guest2MenuViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;

            MyVouchersCommand = new ExecuteMethodCommand(ShowMyVouchersView);
            TourBrowserCommand = new ExecuteMethodCommand(ShowTourBrowserView);
            TrackingCommand = new TourTrackingCommand(ShowTourTrackingView, user);

        }

        private void ShowTourTrackingView()
        {
            CurrentKeyPointViewModel currentKeyPointViewModel = new CurrentKeyPointViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, currentKeyPointViewModel ));

            navigate.Execute(null);
        }

        private void ShowMyVouchersView()
        {
            MyVouchersViewModel myVouchersViewModel = new MyVouchersViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, myVouchersViewModel));

            navigate.Execute(null);
        }

        private void ShowTourBrowserView()
        {
            TourBrowserViewModel tourBrowserViewModel = new TourBrowserViewModel(_navigationStore, _user);
            NavigateCommand navigate = new NavigateCommand(new NavigationService(_navigationStore, tourBrowserViewModel));

            navigate.Execute(null);
        }
    }
}
