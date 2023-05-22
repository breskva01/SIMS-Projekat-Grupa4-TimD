using InitialProject.Application.Injector;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Util
{
    public class SuperGuestHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationReservationRepository _reservationRepository;
        public SuperGuestHandler()
        {
            _userRepository = RepositoryInjector.Get<IUserRepository>();
            _reservationRepository = RepositoryInjector.Get<IAccommodationReservationRepository>();
        }
        public void UpdateSuperGuestStatus()
        {
            var guests = _userRepository.GetAllGuest1s();
            foreach (Guest1 guest in guests)
            {
                if (ShouldUpdateSuperGuestStatus(guest))
                {
                    if (ShouldActivateSuperGuest(guest))
                        guest.ActivateSuperGuest();
                    else
                        guest.CancelSuperGuest();

                    _userRepository.Update(guest);
                }
            }
        }
        private bool ShouldUpdateSuperGuestStatus(Guest1 guest)
        {
            return !guest.SuperGuest || guest.HasSuperGuestExpired();
        }
        private bool ShouldActivateSuperGuest(Guest1 guest)
        {
            var reservations = _reservationRepository.GetFilteredReservations(
                guestId: guest.Id,
                status: AccommodationReservationStatus.Finished
            );

            var viableReservations = reservations.FindAll(r => r.HappenedInLastYear());

            return viableReservations.Count >= 10;
        }
    }
}
