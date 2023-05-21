﻿using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository : IRepository<TourRequest>
    {
        public TourRequest GetById(int tourRequestId);
        public List<TourRequest> GetByUser(int userId);
        public List<TourRequest> GetApproved(List<TourRequest> tourRequests);
        public List<TourRequest> GetOnHold();
        public List<TourRequest> CheckIfInvalid(List<TourRequest> tourRequests);
        public int GetApprovedForYear(string year);
        public int GetApprovedGeneral();
        public int GetGeneralNumberOfGuests();
        public int GetYearNumberOfGuests(string year);
        public int GetYearNumberOfRequests(int year);
        public int GetLanguageNumberOfRequests(GuideLanguage guideLanguage);
        public int GetLocationNumberOfRequests(string city, List<TourRequest> tourRequests);
        public List<string> GetAllLocations(List<TourRequest> tourRequests);
        public List<int> GetAllYears();
        public List<string> GetAvailableYears();
        public TourRequest Update(TourRequest tourRequest);
        public TourRequest Save(TourRequest tourRequest);
        public int NextId();
        public void Delete(TourRequest tourRequest);
        public List<TourRequest> GetFiltered(string country, string city, DateTime date1, DateTime date2, int number, string language);

    }
}
