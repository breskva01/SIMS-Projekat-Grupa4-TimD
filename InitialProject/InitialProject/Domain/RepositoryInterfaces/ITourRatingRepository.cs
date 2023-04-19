using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        public List<TourRating> GetAll();
        public TourRating GetById(int id);
        public TourRating Update(TourRating rating);
        public TourRating Save(TourRating tourRating);
        public int NextId();
    }
}
