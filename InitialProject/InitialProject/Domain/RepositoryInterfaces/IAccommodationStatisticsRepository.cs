using InitialProject.Domain.Models;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationStatisticsRepository
    {
        public LineSeries GetYearlyReservations(int id);
        public LineSeries GetYearlyCancellations(int id);
        public LineSeries GetYearlyMovedReservations(int id);
        public LineSeries GetYearlyRenovationReccommendations(int id);
        public LineSeries GetMonthlyReservations(int id, int year);
        public LineSeries GetMonthlyCancellations(int id, int year);
        public LineSeries GetMonthlyMovedReservations(int id, int year);
        public LineSeries GetMonthlyRenovationReccommendations(int id, int year);
        public string GetMostBookedYear(int id);
        public string GetMostBookedMonth(int id, int year);
        public List<Location> GetMostPopularLocations();
        public List<Location> GetMostUnpopularLocations();
    }
}
