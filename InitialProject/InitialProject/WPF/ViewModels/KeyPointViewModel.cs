using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class KeyPointViewModel : ViewModelBase
    {
        private int _keyPointId;
        private Location _location;
        private int _locationId;
        private string _place;
        private bool _isReached;

        public KeyPointViewModel(KeyPoint keyPoint)
        {
            _keyPointId = keyPoint.Id;
            _location = keyPoint.Location;
            _isReached = keyPoint.Reached;
            _locationId = keyPoint.LocationId;  
            _place = keyPoint.Place;
        }

        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public int LocationId
        {
            get => _locationId;
            set
            {
                _locationId = value;
                OnPropertyChanged(nameof(LocationId));
            }
        }

        public int KeyPointId
        {
            get => _keyPointId;
            set
            {
                _keyPointId = value;
                OnPropertyChanged(nameof(KeyPointId));
            }
        }

        public string Place
        {
            get => _place;
            set
            {
                _place = value;
                OnPropertyChanged(nameof(Place));
            }
        }

        public bool IsReached
        {
            get => _isReached;
            set
            {
                _isReached = value;
                OnPropertyChanged(nameof(IsReached));
            }
        }
    }
}
