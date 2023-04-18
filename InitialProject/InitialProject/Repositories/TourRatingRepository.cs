using InitialProject.Domain;
using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class TourRatingRepository
    {
        private List<TourRating> _tourRatings;
        private readonly TourRatingFileHandler _tourRatingFileHandler;

        public TourRatingRepository()
        {
            _tourRatingFileHandler = new TourRatingFileHandler();
            _tourRatings = _tourRatingFileHandler.Load();
        }

        public List<TourRating> GetAll()
        {
            return _tourRatingFileHandler.Load();
        }
        public TourRating Get(int id)
        {
            return _tourRatings.Find(t => t.Id == id);
        }

        public TourRating Update(TourRating rating)
        {
            _tourRatings = _tourRatingFileHandler.Load();
            TourRating updated = _tourRatings.Find(r => r.Id == rating.Id);
            _tourRatings.Remove(updated);
            _tourRatings.Add(rating);
            _tourRatingFileHandler.Save(_tourRatings);
            return rating;
        }

        public TourRating Save(TourRating tourRating)
        {
            _tourRatings = _tourRatingFileHandler.Load();

            tourRating.Id = NextId();
            _tourRatings.Add(tourRating);
            _tourRatingFileHandler.Save(_tourRatings);

            //NotifyObservers();

            return tourRating;
        }

        public int NextId()
        {
            _tourRatings = _tourRatingFileHandler.Load();
            if (_tourRatings.Count < 1)
            {
                return 1;
            }
            return _tourRatings.Max(r => r.Id) + 1;
        }


    }
}
