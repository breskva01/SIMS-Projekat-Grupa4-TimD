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

        public List<TourReservation> getActivePendingReservations(int userId)
        {
            _reservations = _repository.GetAll();
            //uzmi sve rezervacije koje odgovaraju korinikovom ID

            //od tih rezervacija uzmi samo one koje imaju Pending status

            //od tih rezervacija uzmi one koje imaju aktivnu turu u sebi
            //List<TourReservation> reservations = new();
            //foreach(TourReservation reservation in _reservations)
            //{
            //    if(_tourService.GetById(reservation.TourId).State == TourState.Started)
              //  {
             //       if (reservation.Presence == Presence.Pending)
                 //   {
                 //       reservations.Add(reservation);
                 //   }
               // }
            //}
           // return reservations;
           throw new NotImplementedException();


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
