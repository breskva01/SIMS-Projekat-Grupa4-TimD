using InitialProject.Application.Commands;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels
{
    public class TourCreationTutorialViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourCreationTutorialViewModel(NavigationStore navigationStore, User user)
        {
            _navigationStore = navigationStore;
            _user = user;
        }
    }
}
