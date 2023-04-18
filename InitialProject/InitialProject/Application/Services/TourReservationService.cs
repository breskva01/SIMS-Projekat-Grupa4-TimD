using InitialProject.Application.Observer;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class TourReservationService
    {
        private readonly List<IObserver> _observers;
        private readonly TourReservationRepository _repository;
        private List<TourReservation> _reservations;
        private readonly TourService _tourService;
        
        public TourReservationService()
        {
            _observers = new List<IObserver>();
            _repository = new TourReservationRepository();
            _reservations = new List<TourReservation>();
            _tourService = new TourService();
        }
        public List<TourReservation> GetAll()
        {
            return _repository.GetAll();
        }
        public TourReservation Get(int id)
        {
            return _repository.Get(id);
        }

        public List<TourReservation> GetUnrated(int userId) {
            _reservations = GetByUserId(userId);
            _reservations = GetRateableReservations(_reservations);
            return _reservations;
        }

        public List<TourReservation> GetRateableReservations(List<TourReservation> reservations)
        {
           return _repository.GetRateableReservations(reservations);
        }

        public TourReservation CreateReservation(int tourId, int guestId, int numberOfGuests)
        {
            TourReservation reservation = new()
            {
                TourId = tourId,
                GuestId = guestId,
                NumberOfGuests = numberOfGuests
            };
            return _repository.Save(reservation);
        }
        public TourReservation Update(TourReservation reservation)
        {
            return _repository.Update(reservation);
        }

        public List<TourReservation> getActivePresentReservations(int userId)
        {
            List<TourReservation> activePresentReservations = new();
            _reservations = GetByUserId(userId);
            //uzmi sve rezervacije koje odgovaraju korinikovom ID

            _reservations = GetPresentReservations(_reservations);
            //od tih rezervacija uzmi samo one koje imaju Pending status

            foreach (TourReservation tr in _reservations)
            {
                if (IsActive(tr))
                {
                    activePresentReservations.Add(tr);
                }
            }
            //od tih rezervacija uzmi one koje imaju aktivnu turu u sebi

            return activePresentReservations;
        }

        public List<TourReservation> getActivePendingReservations(int userId)
        {
            List<TourReservation> activePendingReservations = new();
            _reservations = GetByUserId(userId);
            //uzmi sve rezervacije koje odgovaraju korinikovom ID

            _reservations = GetPendingReservations(_reservations);
            //od tih rezervacija uzmi samo one koje imaju Pending status

            foreach(TourReservation tr in _reservations)
            {
                if (IsActive(tr))
                {
                    activePendingReservations.Add(tr);
                }
            }
            //od tih rezervacija uzmi one koje imaju aktivnu turu u sebi

            return activePendingReservations;
        }

        public bool IsActive(TourReservation reservation)
        {
            return _tourService.IsActive(reservation.TourId);
        }

        private List<TourReservation> GetPresentReservations(List<TourReservation> reservations)
        {
            return _repository.GetPresentReservations(reservations);
        }

        private List<TourReservation> GetPendingReservations(List<TourReservation> reservations)
        {
            return _repository.GetPendingReservations(reservations);
        }

        public List<TourReservation> GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId);
        }

        public List<TourReservation> GetByUserAndTourId(int userId, int tourId)
        {
            return _repository.GetByUserAndTourId(userId, tourId);
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
