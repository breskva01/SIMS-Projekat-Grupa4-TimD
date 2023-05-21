using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Application.Util;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repositories.FileHandlers;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationReservationService : Subject
    {
        private readonly IAccommodationReservationRepository _repository;
        public AccommodationReservationService()
        {
            _repository = RepositoryInjector.Get<IAccommodationReservationRepository>();
        }
        public bool IsDiscountAvailable(Guest1 guest)
        {
            bool discountAvailable = guest.SpendABonusPoint();
            if (discountAvailable)
            {
                var userRepository = RepositoryInjector.Get<IUserRepository>();
                userRepository.Update(guest);
            }
            return discountAvailable;
        }
        public void Save(AccommodationReservation accommodationReservation)
        {
            _repository.Save(accommodationReservation);
        }
        public List<AccommodationReservation> GetAll()
        {
            return _repository.GetAll();
        }
        public AccommodationReservation GetById(int reservationId)
        {
            return _repository.GetById(reservationId);
        }
        public List<AccommodationReservation> GetExistingGuestReservations(int guestId)
        {
            return _repository.GetFilteredReservations(guestId: guestId);
        }
        public void Cancel(int reservationId, int ownerId)
        {
            _repository.Cancel(reservationId);
            RepositoryInjector.Get<IAccommodationReservationCancellationNotificationRepository>().
                Save(new AccommodationReservationCancellationNotification(reservationId, ownerId));
            NotifyObservers();
        }
        public List<AccommodationReservation> GetAvailable(DateOnly startDate, DateOnly endDate, int stayLength, Accommodation accommodation, Guest1 guest)
        {
            var reservationAvailabilityHandler = new AccommodationReservationAvailabilityHandler(_repository);
            return reservationAvailabilityHandler.GetAvailable(startDate, endDate, stayLength, accommodation, guest);
        }
        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId)
        {
            return _repository.FindCompletedAndUnrated(ownerId);
        }
        public void MoveReservation(int reservationId, DateOnly newCheckIn, DateOnly newCheckout)
        {
            _repository.MoveReservation(reservationId, newCheckIn, newCheckout);
        }
        public void updateLastNotification(AccommodationReservation accommodationReservation)
        {
            _repository.updateLastNotification(accommodationReservation);
            NotifyObservers();
        }
        public void updateRatingStatus(AccommodationReservation accommodationReservation)
        {
            _repository.updateRatingStatus(accommodationReservation);
            NotifyObservers();
        }
        public string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut)
        {
            return _repository.CheckAvailability(accommodationId ,checkIn, checkOut);
        }
        public List<AccommodationReservation> GetAllNewlyCancelled(int ownerId)
        {
            var notifactions = RepositoryInjector.Get<IAccommodationReservationCancellationNotificationRepository>().
                                    GetByOwnerId(ownerId);
            var cancelledReservatons = new List<AccommodationReservation>();
            notifactions.ForEach(n => 
            {
                cancelledReservatons.Add(_repository.GetById(n.ReservationId));
            });
            return cancelledReservatons;
        }
        public LineSeries GetYearlyReservations(int id)
        {
            return _repository.GetYearlyReservations(id);
        }
        public LineSeries GetYearlyCancellations(int id) 
        {
            return _repository.GetYearlyCancellations(id);
        }
        public LineSeries GetYearlyMovedReservations(int id)
        {
            return _repository.GetYearlyMovedReservations(id);
        }
        public LineSeries GetYearlyRenovationReccommendations(int id)
        {
            return _repository.GetYearlyRenovationReccommendations(id);
        }
        public LineSeries GetMonthlyReservations(int id, int year)
        {
            return _repository.GetMonthlyReservations(id, year);
        }
        public LineSeries GetMonthlyCancellations(int id, int year)
        {
            return _repository.GetMonthlyCancellations(id, year);
        }
        public LineSeries GetMonthlyMovedReservations(int id, int year)
        {
            return _repository.GetMonthlyMovedReservations(id, year);
        }
        public LineSeries GetMonthlyRenovationReccommendations(int id, int year)
        {
            return _repository.GetMonthlyRenovationReccommendations(id, year);
        }
        public string GetMostBookedYear(int id)
        {
            return _repository.GetMostBookedYear(id);
        }
        public string GetMostBookedMonth(int id, int year)
        {
            return _repository.GetMostBookedMonth(id, year);
        }
    }
}
