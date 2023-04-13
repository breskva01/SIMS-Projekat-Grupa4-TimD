using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InitialProject.Application.Commands;
using InitialProject.Domain.Models;

namespace InitialProject.WPF.ViewModels
{
    public class TourBrowserViewModel : ViewModelBase
    {
        private readonly ObservableCollection<TourViewModel> _tours;
        
        public ObservableCollection<TourViewModel> Tours => _tours;
        public TourBrowserViewModel()
        {
            MakeReservationCommand = new MakeReservationCommand();
            _tours = new ObservableCollection<TourViewModel>();
            _tours.Add(new TourViewModel(new Tour(5, "Tura Londonom", 34, "wow tura", GuideLanguage.English, 40, DateTime.Now, 3, "../../Resources/Images/moscow.png", 0, new List<KeyPoint>())));
        }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand ApplySortCommand { get; }
        public ICommand MakeReservationCommand { get; }


    }
}
