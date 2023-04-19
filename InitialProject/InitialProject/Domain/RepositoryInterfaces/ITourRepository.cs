using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();
        public Tour GetById(int tourId);
        public Tour GetByName(String name);
        public List<Tour> GetToursByYear(String year);
        public Tour Update(Tour tour);
        public Tour Save(Tour tour);
        public int NextId();
        public void Delete(Tour tour);
        public bool IsActive(int tourId);
        public List<Tour> GetFiltered(string country, string city, int duration, GuideLanguage language, int numberOfGuests);
        public bool MatchesFilters(Tour tour, string country, string city, int duration, GuideLanguage language, int numberOfGuests);
        public List<Tour> SortByName(List<Tour> tours);
        public List<Tour> SortByLocation(List<Tour> tours);
        public List<Tour> SortByDuration(List<Tour> tours);
        public List<Tour> SortByLanguage(List<Tour> tours);
        public Tour GetMostVisited(String selectedYear);
        public List<string> GetAvailableYears();
        public List<string> GetFinishedTourNames();
        public string GetNumberOfGuestBelow18(Tour tour);
        public string GetNumberOfMiddleAgeGuests(Tour tour);
        public string GetNumberOfOlderGuests(Tour tour);
    }
}
