using InitialProject.Application.Observer;
using InitialProject.Application.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models.DAO
{
    internal class TourReservationDAO : ISubject
    {
        private List<Tour> _tours;
        private List<User> _users;
        private List<TourReservation> _reservations;

        private readonly List<IObserver> _observers;

        private readonly Storage<Tour> _tourStorage;
        private readonly Storage<User> _userStorage;
        private readonly Storage<TourReservation> _reservationStorage;

        private const string TourFilePath = "../../../Resources/Data/tours.csv";
        private const string UserFilePath = "../../../Resources/Data/users.csv";
        private const string ReservationFilePath = "../../../Resources/Data/tourReservations.csv";


        public TourReservationDAO()
        {
            _tourStorage = new Storage<Tour>(TourFilePath);
            _userStorage = new Storage<User>(UserFilePath);
            _reservationStorage = new Storage<TourReservation>(ReservationFilePath);

            _reservations = _reservationStorage.Load();
            _observers = new List<IObserver>();
        }

        public List<TourReservation> GetAll()
        {
            return _reservationStorage.Load();
        }
        public TourReservation Save(TourReservation tourReservation)
        {
            _tours = _tourStorage.Load();
            _users = _userStorage.Load();
            _reservations = _reservationStorage.Load();

            tourReservation.Id = NextId();
            tourReservation.Tour = _tours.FirstOrDefault(t => t.Id == tourReservation.TourId);
            tourReservation.Guest = _users.FirstOrDefault(u => u.Id == tourReservation.GuestId);
            tourReservation.Tour.CurrentNumberOfGuests += tourReservation.NumberOfGuests;

            _tourStorage.Save(_tours);

            _reservations.Add(tourReservation);
            _reservationStorage.Save(_reservations);

            NotifyObservers();

            return tourReservation;
        }
        public int NextId()
        {
            _reservations = _reservationStorage.Load();
            if (_reservations.Count < 1)
            {
                return 1;
            }
            return _reservations.Max(r => r.Id) + 1;
        }

        public void Delete(TourReservation tourReservation)
        {
            _reservations = _reservationStorage.Load();
            TourReservation founded = _reservations.Find(r => r.Id == tourReservation.Id);
            _reservations.Remove(founded);
            _reservationStorage.Save(_reservations);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var o in _observers)
            {
                o.Update();
            }
        }
    }
}
