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
            var users = RepositoryInjector.Get<IUserRepository>().GetAll();
            _accommodations.ForEach(a => a.Owner = users.Find(u => u.Id == a.Owner.Id));
            var accommodations = new List<Accommodation>();
            _accommodations.ForEach(a => { if (a.Owner.SuperOwner) accommodations.Add(a); });
            _accommodations.ForEach(a => { if (!a.Owner.SuperOwner) accommodations.Add(a); });

            return accommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            GetAll();
            accommodation.Id = NextId();
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
            return accommodation;
        }

        private int NextId()
        {
            return _accommodations?.Max(r => r.Id) + 1 ?? 0;
        }

        public void Delete(Accommodation accommodation)
        {
            GetAll();
            Accommodation founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _fileHandler.Save(_accommodations);
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            GetAll();
            var filteredAccommodations = new List<Accommodation>();
            filteredAccommodations = _accommodations.FindAll(a => a.MatchesFilters(keyWords, type, guestNumber, numberOfDays));
            return filteredAccommodations;
        }
        public List<Accommodation> Sort(List<Accommodation> accommodations, string criterium)
        {
            switch (criterium)
            {
                case "Name":
                    return SortByName(accommodations);
                case "Location":
                    return SortByLocation(accommodations);
                case "MaxGuestNumber":
                    return SortByMaxGuestNumber(accommodations);
                case "MinDaysNumber":
                    return SortByMinDaysNumber(accommodations);
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
        private List<Accommodation> SortByMaxGuestNumber(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MaximumGuests).ToList();
        }
        private List<Accommodation> SortByMinDaysNumber(List<Accommodation> accommodations)
        {
            return accommodations.OrderBy(a => a.MinimumDays).ToList();
        }
        public void Add(string name, string country, string city, string address, AccommodationType type, int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL,
                        User owner)
        {
            GetAll();
            int accommodationId = NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, address,
                type, maximumGuests, minimumDays, minimumCancelationNotice,
                                                            pictureURL, owner);
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
        }

    }
}
