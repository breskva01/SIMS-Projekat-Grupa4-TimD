﻿using InitialProject.Application.Serializer;
using InitialProject.Application.Storage;
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
            _accommodations = _fileHandler.Load();
        }

        public List<Accommodation> GetAll()
        {
            return _fileHandler.Load();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _fileHandler.Load();
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
            return accommodation;
        }

        private int NextId()
        {
            _accommodations = _fileHandler.Load();
            return _accommodations?.Max(r => r.Id) + 1 ?? 0;
        }

        public void Delete(Accommodation accommodation)
        {
            _accommodations = _fileHandler.Load();
            Accommodation founded = _accommodations.Find(a => a.Id == accommodation.Id);
            _accommodations.Remove(founded);
            _fileHandler.Save(_accommodations);
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            _accommodations = _fileHandler.Load();
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
                        User owner, int ownerId)
        {
            _accommodations = _fileHandler.Load();
            int accommodationId = NextId();
            Accommodation accommodation = new Accommodation(accommodationId, name, country, city, address,
                type, maximumGuests, minimumDays, minimumCancelationNotice,
                                                            pictureURL, owner, ownerId);
            _accommodations.Add(accommodation);
            _fileHandler.Save(_accommodations);
        }

    }
}
