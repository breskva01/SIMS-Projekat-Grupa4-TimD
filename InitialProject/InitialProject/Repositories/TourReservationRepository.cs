using InitialProject.Application.Observer;
using InitialProject.Application.Storage;
using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class TourReservationRepository
    {
        private List<Tour> _tours;
        private List<User> _users;
        private List<TourReservation> _tourReservations;

        //private readonly List<IObserver> _observers;

        private readonly UserFileHandler _userFileHandler;
        private readonly TourFileHandler _tourFileHandler;
        private readonly TourReservationFileHandler _tourReservationFileHandler;
        
        

        public TourReservationRepository()
        {
            _tourReservationFileHandler = new TourReservationFileHandler();
            _userFileHandler = new UserFileHandler();
            _tourFileHandler = new TourFileHandler();
            _tourReservations = _tourReservationFileHandler.Load();
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationFileHandler.Load();
        }
        public TourReservation Get(int id)
        {
            return _tourReservations.Find(x => x.Id == id);
        }

        public TourReservation Update(TourReservation reservation)
        {
            _tourReservations = _tourReservationFileHandler.Load();
            TourReservation updated = _tourReservations.Find(t => t.Id == reservation.Id);
            _tourReservations.Remove(updated);
            _tourReservations.Add(reservation);
            _tourReservationFileHandler.Save(_tourReservations);
            return reservation;
        }

        public List<TourReservation> GetPresentReservations(List<TourReservation> reservations)
        {
            List<TourReservation> presentReservations = new List<TourReservation>();
            foreach (TourReservation tr in reservations)
            {
                if (tr.Presence == Presence.Present)
                    presentReservations.Add(tr);
            }
            return presentReservations;

        }

        public List<TourReservation> GetPendingReservations(List<TourReservation> reservations)
        {
            List<TourReservation> pendingReservations = new List<TourReservation>();
            foreach (TourReservation tr in reservations)
            {
                if (tr.Presence == Presence.Pending)
                    pendingReservations.Add(tr);
            }
            return pendingReservations;

        }

        public List<TourReservation> GetByUserId(int userId)
        {
            _tourReservations = _tourReservationFileHandler.Load();
            List<TourReservation> reservations = new List<TourReservation>();
            foreach(TourReservation tr in _tourReservations)
            {
                if(tr.GuestId == userId) 
                    reservations.Add(tr);
            }
            return reservations;

        }

        public TourReservation Save(TourReservation tourReservation)
        {
            _tours = _tourFileHandler.Load();
            _users = _userFileHandler.Load();
            _tourReservations = _tourReservationFileHandler.Load();

            tourReservation.Id = NextId();
            tourReservation.Tour = _tours.FirstOrDefault(t => t.Id == tourReservation.TourId);
            tourReservation.Guest = _users.FirstOrDefault(u => u.Id == tourReservation.GuestId);
            tourReservation.Tour.CurrentNumberOfGuests += tourReservation.NumberOfGuests;

            _tourFileHandler.Save(_tours);

            _tourReservations.Add(tourReservation);
            _tourReservationFileHandler.Save(_tourReservations);

            //NotifyObservers();

            return tourReservation;
        }

        public int NextId()
        {
            _tourReservations = _tourReservationFileHandler.Load();
            if (_tourReservations.Count < 1)
            {
                return 1;
            }
            return _tourReservations.Max(r => r.Id) + 1;
        }
    }
}
