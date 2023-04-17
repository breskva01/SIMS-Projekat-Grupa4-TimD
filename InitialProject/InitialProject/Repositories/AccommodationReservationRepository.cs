using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private List<AccommodationReservation> _reservations;
        private readonly AccommodationReservationFileHandler _fileHandler;
        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            _reservations = _fileHandler.Load();
        }
        public List<AccommodationReservation> GetAll()
        {
            return _fileHandler.Load();
        }
        public AccommodationReservation GetById(int reservationId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.Find(r => r.Id == reservationId);
        }
        public List<AccommodationReservation> GetConfirmed(int guestId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.GuestId == guestId &&
                                         r.Status == AccommodationReservationStatus.Confirmed);
        }
        public List<AccommodationReservation> GetExisting(int accommodationId)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.AccommodationId == accommodationId &&
                                         r.Status == AccommodationReservationStatus.Confirmed);
        }
        public List<AccommodationReservation> GetExistingInsideDateRange(int accommodationId, DateOnly startDate, DateOnly endDate)
        {
            _reservations = _fileHandler.Load();
            return _reservations.FindAll(r => r.AccommodationId == accommodationId &&
                                         r.Status == AccommodationReservationStatus.Confirmed &&
                                         r.CheckIn >= startDate && r.CheckOut <= endDate);
        }
        public void Cancel(int reservationId)
        {
            var reservation = GetById(reservationId);         
            reservation.Status = AccommodationReservationStatus.Cancelled;
            _fileHandler.Save(_reservations);
        }
        public void Save(AccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _reservations = _fileHandler.Load();
            _reservations.Add(reservation);
            _fileHandler.Save(_reservations);
        }
        private int NextId()
        {
            _reservations = _fileHandler.Load();
            return _reservations?.Max(r => r.Id) + 1 ?? 0;
            //return _reservations.Count == 0 ? 0 : _reservations.Max(r => r.Id) + 1;
        }
    }
}
