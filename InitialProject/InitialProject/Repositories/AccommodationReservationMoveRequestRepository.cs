using InitialProject.Application.Injector;
using InitialProject.Application.Stores;
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
    public class AccommodationReservationMoveRequestRepository : IAccommodationReservationMoveRequestRepository
    {
        private readonly AccommodationReservationMoveRequestFileHandler _fileHandler;
        private List<AccommodationReservationMoveRequest> _requests;

        public AccommodationReservationMoveRequestRepository()
        {
            _fileHandler = new AccommodationReservationMoveRequestFileHandler();
        }
        public List<AccommodationReservationMoveRequest> GetAll()
        {
            return _requests = _fileHandler.Load();
        }
        public List<AccommodationReservationMoveRequest> GetByOwnerId(int ownerId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId);
        }
        public List<AccommodationReservationMoveRequest> GetByGuestId(int guestId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.Guest.Id == guestId);
        }
        public List<AccommodationReservationMoveRequest> GetAllNewlyAnswered(int guestId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.Guest.Id == guestId &&
                                          r.Status != ReservationMoveRequestStatus.Pending &&
                                          r.GuestNotified == false);
        }
        public void UpdateGuestNotifiedField(int guestId)
        {
            GetAll();
            _requests.FindAll(r => r.Reservation.Guest.Id == guestId &&
                                   r.Status != ReservationMoveRequestStatus.Pending)
                     .ForEach(r => r.GuestNotified = true);
            _fileHandler.Save(_requests);
        }
        public List<AccommodationReservationMoveRequest> GetPendingRequestsByOwnerId(int ownerId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.Accommodation.Owner.Id == ownerId && 
                                          r.Status == ReservationMoveRequestStatus.Pending);
        }
        public void ApproveRequest(int reservationId)
        {
            GetAll();
            AccommodationReservationMoveRequest request = _requests.Find(r => r.Reservation.Id == reservationId);
            AccommodationReservationMoveRequest newRequest = request;
            newRequest.Status = ReservationMoveRequestStatus.Accepted;
            _requests.Remove(request);
            _requests.Add(newRequest);
            _fileHandler.Save(_requests);
        }
        public void DenyRequest(int reservationId, string comment)
        {
            GetAll();
            AccommodationReservationMoveRequest request = _requests.Find(r => r.Reservation.Id == reservationId);
            AccommodationReservationMoveRequest newRequest = request;
            newRequest.Status = ReservationMoveRequestStatus.Declined;
            newRequest.Comment = comment;
            _requests.Remove(request);
            _requests.Add(newRequest);
            _fileHandler.Save(_requests);
        }
        public void Save(AccommodationReservationMoveRequest request)
        {
            GetAll();
            _requests.Add(request);
            _fileHandler.Save(_requests);
        }
    }
}
