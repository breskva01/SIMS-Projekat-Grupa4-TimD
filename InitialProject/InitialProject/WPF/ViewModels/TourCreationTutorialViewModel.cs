using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.WPF.NewViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class TourCreationTutorialViewModel
    {
        private readonly NavigationStore _navigationStore;
        private User _user;

        public TourCreationTutorialViewModel(NavigationStore navigationStore, User user, TourCreationTutorialView view)
        {
            _navigationStore = navigationStore;
            _user = user;

        }  
    }
}
