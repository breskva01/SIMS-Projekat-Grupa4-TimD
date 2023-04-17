using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    class TourReservationRepository
    {
        private readonly TourReservationFileHandler _fileHandler;
        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _fileHandler = new TourReservationFileHandler();
            _tourReservations = _fileHandler.Load();
        }

        public List<TourReservation> GetAll()
        {
            return _fileHandler.Load();
        }
        public TourReservation Get(int id)
        {
            return _tourReservations.Find(x => x.Id == id);
        }
    }
}
