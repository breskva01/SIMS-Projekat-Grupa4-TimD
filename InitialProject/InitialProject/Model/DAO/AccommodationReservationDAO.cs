using InitialProject.Observer;
using InitialProject.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DAO
{
    public class AccommodationReservationDAO : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly Storage<AccommodationReservation> _storage;
        private List<AccommodationReservation> _reservations;
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";
        private const string AccommodationsFilePath = "../../../Resources/Data/accommodations.csv";
        private const string GuestsFilePath = "../../../Resources/Data/users.csv";

        public AccommodationReservationDAO()
        {
            _storage = new Storage<AccommodationReservation>(FilePath);
            _reservations = _storage.Load();
            _observers = new List<IObserver>();
        }
        public List<AccommodationReservation> GetAll()
        {
            _reservations = _storage.Load();
            FillInAccommodations();
            FillInGuests();
            return _reservations;
        }
        private void FillInAccommodations() 
        {
            Storage<Accommodation> storage = new Storage<Accommodation>(AccommodationsFilePath);
            List<Accommodation> accommodations = storage.Load();
            foreach(var reservation in _reservations)
            {
                reservation.Accommodation = accommodations.FirstOrDefault(a => a.Id == reservation.AccommodationId);
            }
        }
        private void FillInGuests()
        {
            Storage<User> storage = new Storage<User>(GuestsFilePath);
            List<User> guests = storage.Load();
            foreach (var reservation in _reservations)
            {
                reservation.Guest = guests.FirstOrDefault(g => g.Id == reservation.GuestId);
            }
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
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
