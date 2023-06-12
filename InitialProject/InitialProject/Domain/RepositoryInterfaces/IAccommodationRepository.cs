﻿using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository : IRepository<Accommodation>
    {
        List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays);
        List<Accommodation> Sort(List<Accommodation> accommodations, string criterium);
        public void RegisterAccommodation(string name, Location location, string address, AccommodationType type, 
            int maximumGuests, int minimumDays, int minimumCancelationNotice, List<string> pictureURLs, User owner);
        public List<Accommodation> GetAllOwnersAccommodations(int id);
        public void UpdateRenovationStatus(int id);
    }
}
