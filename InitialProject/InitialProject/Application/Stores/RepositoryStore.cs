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
        private static IUserRepository _userRepository;
        private static IAccommodationRepository _accommodationRepository;
        private static IAccommodationReservationRepository _accommodationReservationRepository;
        private static IAccommodationRatingRepository _accommodationRatingRepository;
        private static IAccommodationReservationCancellationNotificationRepository _accommodationReservationCancellationNotificationRepository;
        private static IAccommodationReservationMoveRequestRepository _accommodationReservationMoveRequestRepository;
        private static ITourRepository _tourRepository;
        private static ITourReservationRepository _tourReservationRepository;
        private static ITourRatingRepository _tourRatingRepository;
        private static IVoucherRepository _voucherRepository;
        private static ILocationRepository _locationRepository;
        private static IKeyPointRepository _keyPointRepository;
        private static IGuestRatingRepository _guestRatingRepository;

        public static IAccommodationReservationMoveRequestRepository GetIAccommodationReservationMoveRequestRepository
        {
            get
            {
                if (_accommodationReservationMoveRequestRepository == null)
                {
                    _accommodationReservationMoveRequestRepository = new AccommodationReservationMoveRequestRepository();
                }
                return _accommodationReservationMoveRequestRepository;
            }
        }
        public static IAccommodationReservationCancellationNotificationRepository GetIAccommodationReservationCancellationNotificationRepository
        {
            get
            {
                if (_accommodationReservationCancellationNotificationRepository == null)
                {
                    _accommodationReservationCancellationNotificationRepository = new AccommodationReservationCancellationNotificationRepository();
                }
                return _accommodationReservationCancellationNotificationRepository;
            }
        }
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

        public static ITourRepository GetITourRepository
        {
            get
            {
                if (_tourRepository == null)
                {
                    _tourRepository = new TourRepository();
                }
                return _tourRepository;
            }
        }

        public static ITourReservationRepository GetITourReservationRepository
        {
            get
            {
                if (_tourReservationRepository == null)
                {
                    _tourReservationRepository = new TourReservationRepository();
                }
                return _tourReservationRepository;
            }
        }

        public static IVoucherRepository GetIVoucherRepository
        {
            get
            {
                if (_voucherRepository == null)
                {
                    _voucherRepository = new VoucherRepository();
                }
                return _voucherRepository;
            }
        }

        public static ITourRatingRepository GetITourRatingRepository
        {
            get
            {
                if (_tourRatingRepository == null)
                {
                    _tourRatingRepository = new TourRatingRepository();
                }
                return _tourRatingRepository;
            }
        }

        public static ILocationRepository GetILocationRepository
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new LocationRepository();
                }
                return _locationRepository;
            }
        }

        public static IKeyPointRepository GetIKeyPointRepository
        {
            get
            {
                if (_keyPointRepository == null)
                {
                    _keyPointRepository = new KeyPointRepository();
                }
                return _keyPointRepository;
            }
        }
        public static IGuestRatingRepository GetIGuestRatingRepository
        {
            get
            {
                if (_guestRatingRepository == null)
                {
                    _guestRatingRepository = new GuestRatingRepository();
                }
                return _guestRatingRepository;
            }
        }
    }

}
