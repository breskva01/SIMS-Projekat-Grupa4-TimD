using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public TourReservation Update(TourReservation tourReservation);
        public List<TourReservation> GetAll();
        public TourReservation GetById(int id);
        public List<TourReservation> GetPresent(List<TourReservation> reservations);
        public List<TourReservation> GetPending(List<TourReservation> reservations);
        public List<TourReservation> GetDuplicates(List<TourReservation> reservations, int tourId);
        public List<TourReservation> GetUnrated(List<TourReservation> reservations);
        public List<TourReservation> GetByUserId(int userId);
        public List<TourReservation> GetByUserAndTourId(int userId, int tourId);
        public TourReservation Save(TourReservation tourReservation);
        public int NextId();
        public List<TourReservation> GetPresentByTourId(int id);
        public string GetVoucherPercentage(int id);
        public List<TourReservation> GetRatedByTourId(int id);
    }
}
