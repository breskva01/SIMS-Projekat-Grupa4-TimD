using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRatingRepository
    {
        List<AccommodationRating> GetAll();
        List<AccommodationRating> GetByOwnerId(int ownerId);
        List<AccommodationRating> GetEgligibleForDisplay(int ownerId);
        void Save(AccommodationRating rating);
    }

}
