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
        private readonly IAccommodationReservationRepository _reservationRepository;
        public AccommodationReservationService()
        {
            _reservationRepository = RepositoryInjector.Get<IAccommodationReservationRepository>();
        }
        public void Save(AccommodationReservation accommodationReservation)
        {
            _reservationRepository.Save(accommodationReservation);
        }
        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }
        public AccommodationReservation GetById(int reservationId)
        {
            return _reservationRepository.GetById(reservationId);
        }
        public List<AccommodationReservation> GetExistingGuestReservations(int guestId)
        {
            return _reservationRepository.GetFilteredReservations(guestId: guestId);
        }
        public void Cancel(int reservationId, int ownerId)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            reservation.Status = AccommodationReservationStatus.Cancelled;
            _reservationRepository.Update(reservation);
            RepositoryInjector.Get<IAccommodationReservationCancellationNotificationRepository>().
                Save(new AccommodationReservationCancellationNotification(reservationId, ownerId));
            NotifyObservers();
        }
        public List<AccommodationReservation> GetAvailable(DateOnly startDate, DateOnly endDate, int stayLength, Accommodation accommodation, Guest1 guest)
        {
            var reservationAvailabilityHandler = new AccommodationReservationAvailabilityHandler(_reservationRepository);
            return reservationAvailabilityHandler.GetAvailable(startDate, endDate, stayLength, accommodation, guest);
        }
        public List<AccommodationReservation> FindCompletedAndUnrated(int ownerId)
        {
            return _reservationRepository.FindCompletedAndUnrated(ownerId);
        }
        public void MoveReservation(int reservationId, DateOnly newCheckIn, DateOnly newCheckout)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            reservation.CheckIn = newCheckIn;
            reservation.CheckOut = newCheckout;
            _reservationRepository.Update(reservation);
        }
        public void UpdateLastNotification(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.LastNotification = accommodationReservation.LastNotification.AddDays(1);
            _reservationRepository.Update(accommodationReservation);
        }
        public void MarkGuestAsRated(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.IsGuestRated = true;
            _reservationRepository.Update(accommodationReservation);
        }
        public string CheckAvailability(int accommodationId, DateOnly checkIn, DateOnly checkOut)
        {
            return _reservationRepository.CheckAvailability(accommodationId ,checkIn, checkOut);
        }
        public List<AccommodationReservation> GetAllNewlyCancelled(int ownerId)
        {
            var notifactions = RepositoryInjector.Get<IAccommodationReservationCancellationNotificationRepository>().
                                    GetByOwnerId(ownerId);
            var cancelledReservatons = new List<AccommodationReservation>();
            notifactions.ForEach(n => 
            {
                cancelledReservatons.Add(_reservationRepository.GetById(n.ReservationId));
            });
            return cancelledReservatons;
        }
        public List<AccommodationReservation> GetAnywhereAnytime(int guestCount, int stayLength, Guest1 guest,
            DateOnly? startDate = null, DateOnly? endDate = null)
        {
            startDate ??= DateOnly.FromDateTime(DateTime.Now);
            endDate ??= startDate.Value.AddYears(1);
            var reservationAvailabilityHandler = new AccommodationReservationAvailabilityHandler(_reservationRepository);
            var availableReservations = new List<AccommodationReservation>();
            var accommodations = new AccommodationService().GetFiltered("", AccommodationType.Everything, guestCount, stayLength);

            foreach (var accommodation in accommodations)
            {
                var reservations = reservationAvailabilityHandler.GetAvailable(
                    startDate.Value, endDate.Value, stayLength, accommodation, guest, 1, false);

                availableReservations.AddRange(reservations);
            }
            return availableReservations;
        }
        public List<TimeSlot> GetAvailableDates(DateTime start, DateTime end, int duration, int id)
        {
            return _reservationRepository.GetAvailableDates(start, end, duration, id);
        }
    }
}
