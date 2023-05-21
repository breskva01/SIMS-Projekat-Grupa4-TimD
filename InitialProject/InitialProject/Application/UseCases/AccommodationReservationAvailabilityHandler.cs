using InitialProject.Application.Injector;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.UseCases
{
    public class AccommodationReservationAvailabilityHandler
    {
        private DateOnly _startDate;
        private DateOnly _endDate;
        private int _stayLength;
        private Accommodation _accommodation;
        private Guest1 _guest;
        private List<AccommodationReservation> _existingReservations;
        private List<AccommodationReservation> _availableReservations;
        private DateOnly _currentDate => DateOnly.FromDateTime(DateTime.Now);

        private readonly IAccommodationReservationRepository _repository;
        public AccommodationReservationAvailabilityHandler(IAccommodationReservationRepository repository)
        {
            _repository = repository;
        }
        private void StoreParamaters(DateOnly startDate, DateOnly endDate, int stayLength, Accommodation accommodation, Guest1 guest)
        {
            _startDate = startDate;
            _endDate = endDate;
            _stayLength = stayLength;
            _accommodation = accommodation;
            _guest = guest;
            _existingReservations = _repository.GetFilteredReservations(accommodationId: _accommodation.Id);
            _availableReservations = new List<AccommodationReservation>();
        }
        public List<AccommodationReservation> GetAvailable(DateOnly startDate, DateOnly endDate, int stayLength,
            Accommodation accommodation, Guest1 guest, int maxReservationCount = 3)
        {
            StoreParamaters(startDate, endDate, stayLength, accommodation, guest);
            FindInsideDateRange(maxReservationCount);
            if (_availableReservations.Count == 0)
            {
                FindOutsideDateRange(maxReservationCount);
            }   
            return _availableReservations;
        }
        private void FindInsideDateRange(int maxReservationCount)
        {
            DateOnly checkIn = _startDate;
            DateOnly checkOut = _startDate.AddDays(_stayLength);

            while (_availableReservations.Count < maxReservationCount && checkOut <= _endDate)
            {
                if (IsAvailable(checkIn, checkOut))
                {
                    _availableReservations.Add(new AccommodationReservation(_accommodation, _guest, checkIn, checkOut));
                }
                checkIn = checkIn.AddDays(1);
                checkOut = checkOut.AddDays(1);
            }
        }
        private void FindOutsideDateRange(int maxReservationCount)
        {
            SetStartingSearchDates();
            bool isBeforeDateRange = true;
            int daysOffset = 0;

            while (_availableReservations.Count < maxReservationCount)
            {
                DateOnly checkIn = isBeforeDateRange ? _startDate.AddDays(-daysOffset) : _endDate.AddDays(daysOffset);
                DateOnly checkOut = checkIn.AddDays(_stayLength);
                if (IsAvailable(checkIn, checkOut))
                {
                    _availableReservations.Add(new AccommodationReservation(_accommodation, _guest, checkIn, checkOut));
                }
                daysOffset++;
                isBeforeDateRange = !isBeforeDateRange;
            }
        }
        private void SetStartingSearchDates()
        {
            var reservationsInsideDateRange = _repository.GetFilteredReservations(accommodationId: _accommodation.Id,
                                                                                  startDate: _startDate, endDate: _endDate);
            if (reservationsInsideDateRange.Count == 0)
                _endDate = _startDate.AddDays(1);
            else
            {
                _startDate = reservationsInsideDateRange.Min(r => r.CheckIn).AddDays(-_stayLength);
                _endDate = reservationsInsideDateRange.Max(r => r.CheckOut);
            }
        }
        private bool IsAvailable(DateOnly checkIn, DateOnly checkOut)
        {
            return !(checkIn < _currentDate || _existingReservations.Any(r => r.Overlaps(checkIn, checkOut)));
        }
    }
}
