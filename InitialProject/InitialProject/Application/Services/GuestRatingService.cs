﻿using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Application.Stores;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Application.Injector;

namespace InitialProject.Application.Services
{
    public class GuestRatingService
    {
        private readonly IGuestRatingRepository _ratingRepository;

        public GuestRatingService()
        {
            _ratingRepository = RepositoryInjector.Get<IGuestRatingRepository>();
        }

        public List<GuestRating> GetAll()
        {
            return _ratingRepository.GetAll();
        }
        public GuestRating RateGuest(int ownerId, int guestId, int hygiene, int respectsRules, int communication, int timeliness, int noiseLevel, int overallExperience, string comment)
        {
            return _ratingRepository.Add(ownerId, guestId, hygiene, respectsRules, communication, timeliness, noiseLevel, overallExperience, comment);
        }
        public List<GuestRating> GetEligibleForDisplay(int guestId)
        {
            return _ratingRepository.GetEligibleForDisplay(guestId);
        }
    }
}
