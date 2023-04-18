﻿using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Stores
{
    public class RepositoryStore
    {
        private static IUserRepository _userRepository;
        private static IAccommodationRepository _accommodationRepository;
        private static IAccommodationReservationRepository _accommodationReservationRepository;
        private static IAccommodationRatingRepository _accommodationRatingRepository;

        public static IUserRepository GetIUserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository();
                }
                return _userRepository;
            }
        }

        public static IAccommodationRepository GetIAccommodationRepository
        {
            get
            {
                if (_accommodationRepository == null)
                {
                    _accommodationRepository = new AccommodationRepository();
                }
                return _accommodationRepository;
            }
        }

        public static IAccommodationReservationRepository GetIAccommodationReservationRepository
        {
            get
            {
                if (_accommodationReservationRepository == null)
                {
                    _accommodationReservationRepository = new AccommodationReservationRepository();
                }
                return _accommodationReservationRepository;
            }
        }
        public static IAccommodationRatingRepository GetIAccommodationRatingRepository
        {
            get
            {
                if (_accommodationRatingRepository == null)
                {
                    _accommodationRatingRepository = new AccommodationRatingRepository();
                }
                return _accommodationRatingRepository;
            }
        }
    }

}
