using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
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
        private readonly ITourReservationRepository _repository;
        private List<TourReservation> _reservations;
        private readonly TourService _tourService;
        
        public TourReservationService()
        {
            _observers = new List<IObserver>();
            _repository = RepositoryStore.GetITourReservationRepository;
            _reservations = new List<TourReservation>();
            _tourService = new TourService();
        }

        public List<TourReservation> GetAll()
        {
            return _repository.GetAll();
        }
        public TourReservation GetById(int id)
        {
            return _repository.GetById(id);
        }
        public List<TourReservation> GetByUserId(int userId)
        {
            return _repository.GetByUserId(userId);
        }
        public List<TourReservation> GetByUserAndTourId(int userId, int tourId)
        {
            return _repository.GetByUserAndTourId(userId, tourId);
        }
        public TourReservation Update(TourReservation reservation)
        {
            return _repository.Update(reservation);
        }
        public TourReservation CreateReservation(int tourId, int guestId, int numberOfGuests, bool usedVoucher)
        {
            TourReservation reservation = new()
            {
                TourId = tourId,
                GuestId = guestId,
                NumberOfGuests = numberOfGuests,
                UsedVoucher = usedVoucher
            };
            return _repository.Save(reservation);
        }

        public List<TourReservation> GetUnratedByUser(int userId) {
            _reservations = GetByUserId(userId);
            _reservations = GetUnrated(_reservations);
            return _reservations;
        }
        public List<TourReservation> GetUnrated(List<TourReservation> reservations)
        {
           return _repository.GetUnrated(reservations);
        }

        public List<TourReservation> GetActivePresent(int userId)
        {
            List<TourReservation> activePresentReservations = new();

            _reservations = GetByUserId(userId);
            _reservations = GetPresent(_reservations);

            foreach (TourReservation tr in _reservations)
            {
                if (IsActive(tr))
                {
                    activePresentReservations.Add(tr);
                }
            }

            return activePresentReservations;
        }
        public List<TourReservation> GetActivePending(int userId)
        {
            List<TourReservation> activePendingReservations = new();

            _reservations = GetByUserId(userId);
            _reservations = GetPending(_reservations);

            foreach(TourReservation tr in _reservations)
            {
                if (IsActive(tr))
                {
                    activePendingReservations.Add(tr);
                }
            }

            return activePendingReservations;
        }
        public bool IsActive(TourReservation reservation)
        {
            return _tourService.IsActive(reservation.TourId);
        }
        private List<TourReservation> GetPresent(List<TourReservation> reservations)
        {
            return _repository.GetPresentReservations(reservations);
        }
        private List<TourReservation> GetPending(List<TourReservation> reservations)
        {
            return _repository.GetPendingReservations(reservations);
        }

        public List<TourReservation> GetDuplicates(int userId, int tourId)
        {
            _reservations = GetActivePending(userId);
            _reservations = _repository.GetDuplicates(_reservations, tourId);
            return _reservations;

        }
        public List<TourReservation> GetPresentByTourId(int tourId)
        {
            return _repository.GetPresentByTourId(tourId);
        }
        public string GetVoucherPercentage(int id)
        {
            return _repository.GetVoucherPercentage(id);
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
