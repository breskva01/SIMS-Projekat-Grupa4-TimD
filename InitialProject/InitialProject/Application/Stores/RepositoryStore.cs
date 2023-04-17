using InitialProject.Domain.RepositoryInterfaces;
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
        private static UserRepository _userRepository;
        private static AccommodationRepository _accommodationRepository;
        private static AccommodationReservationRepository _accommodationReservationRepository;

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
    }

}
