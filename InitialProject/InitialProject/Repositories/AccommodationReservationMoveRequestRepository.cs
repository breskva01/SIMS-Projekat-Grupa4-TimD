﻿using InitialProject.Application.Stores;
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
            _requests = _fileHandler.Load();
        }
        public List<AccommodationReservationMoveRequest> GetAll()
        {
            _requests = _fileHandler.Load();
            var reservations = RepositoryStore.GetIAccommodationReservationRepository.GetAll();
            _requests.ForEach(req => 
                                req.Reservation = reservations.Find
                                    (res => res.Id == req.ReservationId)
                             );
            return _requests;
        }
        public List<AccommodationReservationMoveRequest> GetByOwnerId(int ownerId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.Accommodation.OwnerId == ownerId);
        }
        public List<AccommodationReservationMoveRequest> GetByGuestId(int guestId)
        {
            GetAll();
            return _requests.FindAll(r => r.Reservation.GuestId == guestId);
        }
        public List<AccommodationReservationMoveRequest> GetAllNewlyAnswered(int guestId)
        {
            GetAll();
            var answeredRequests = _requests.FindAll(r => r.Reservation.GuestId == guestId &&
                                          r.Status != ReservationMoveRequestStatus.Pending &&
                                          r.GuestNotified == false);
            UpdateGuestNotifiedField(guestId);
            return answeredRequests;
        }
        private void UpdateGuestNotifiedField(int guestId)
        {
            _requests.FindAll(request => request.Reservation.GuestId == guestId &&
                                         request.Status != ReservationMoveRequestStatus.Pending)
                     .ForEach(request => request.GuestNotified = true);
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
