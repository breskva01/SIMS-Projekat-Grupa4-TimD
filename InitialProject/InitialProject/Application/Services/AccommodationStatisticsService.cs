using InitialProject.Application.Injector;
using InitialProject.Domain.RepositoryInterfaces;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationStatisticsService
    {
        private readonly IAccommodationStatisticsRepository _repository;
        public AccommodationStatisticsService()
        {
            _repository = RepositoryInjector.Get<IAccommodationStatisticsRepository>();
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
