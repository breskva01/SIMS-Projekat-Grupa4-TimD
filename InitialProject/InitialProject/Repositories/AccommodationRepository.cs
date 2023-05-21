using InitialProject.Application.Injector;
using InitialProject.Application.Serializer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AccommodationFileHandler _fileHandler;
        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _fileHandler = new AccommodationFileHandler();
        }

        public List<Accommodation> GetAll()
        {
            _accommodations = _fileHandler.Load();
            return _accommodations.OrderBy(a => !a.Owner.SuperOwner).ToList();
        }
        private int NextId()
        {
            return _accommodations?.Max(r => r.Id) + 1 ?? 0;
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            GetAll();
            return _accommodations.FindAll
                (a => a.MatchesFilters(keyWords, type, guestNumber, numberOfDays));
        }
        public List<Accommodation> Sort(List<Accommodation> accommodations, string criterion)
        {
            switch (criterion)
            {
                case "Name":
                    return SortByName(accommodations);
                case "Location":
                    return SortByLocation(accommodations);
                case "MaxGuestCountAsc":
                    return SortByMaxGuestCount(accommodations);
                case "MaxGuestCountDesc":
                    {
                        var sortedList = SortByMaxGuestCount(accommodations);
                        sortedList.Reverse();
                        return sortedList;
                    }
                case "MinDaysNumberAsc":
                    return SortByMinDaysNumber(accommodations);
                case "MinDaysNumberDesc":
                    {
                        var sortedList = SortByMinDaysNumber(accommodations);
                        sortedList.Reverse();
                        return sortedList;
                    }
                default:
                    return accommodations;
            }
        }
        private List<Accommodation> SortByName(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.Name).ToList();
        }
        private List<Accommodation> SortByLocation(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.Country)
                                          .ThenBy(a => a.City)
                                          .ToList();
        }
        private List<Accommodation> SortByMaxGuestCount(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MaximumGuests).ToList();
        }
        private List<Accommodation> SortByMinDaysNumber(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MinimumDays).ToList();
        }
        public void Add(string name, string country, string city, string address, AccommodationType type,
            int maximumGuests, int minimumDays, int minimumCancelationNotice, List<string> pictureURLs, User owner)
        {
            GetAll();
            int accommodationId = NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, address,
                type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURLs, (Owner)owner);
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
        }
        public List<Accommodation> GetAllOwnersAccommodations(int id)
        {
            List<Accommodation> allOwnersAccommodations= new List<Accommodation>();
            foreach(Accommodation accommodation in _accommodations)
            {
                if(accommodation.OwnerId==id)
                    allOwnersAccommodations.Add(accommodation);
            }
            return allOwnersAccommodations;
        }
    }
}
