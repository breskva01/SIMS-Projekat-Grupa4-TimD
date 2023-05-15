using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        List<Accommodation> GetAll();
        Accommodation Save(Accommodation accommodation);
        void Delete(Accommodation accommodation);
        List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays);
        List<Accommodation> Sort(List<Accommodation> accommodations, string criterium);
        public void Add(string name, string country, string city, string address, AccommodationType type, int maximumGuests, int minimumDays, int minimumCancelationNotice, string pictureURL,
                        User owner, int ownerId);
        public List<Accommodation> GetAllOwnersAccommodations(int id);
    }
}
