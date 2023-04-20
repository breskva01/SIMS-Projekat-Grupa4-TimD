using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRatingRepository : IRepository<AccommodationRating>
    {
        List<AccommodationRating> GetByOwnerId(int ownerId);
        List<AccommodationRating> GetEligibleForDisplay(int ownerId);
        void Save(AccommodationRating rating);
    }

}
