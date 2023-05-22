using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public void ScheduleRenovation(int id, DateTime start, DateTime end, string description);
        public List<AccommodationRenovation> GetAllAppointmentsByOwner(int id);
        public void UpdateStatus(int id);
        public void CancelAppointment(int id);
    }
}
