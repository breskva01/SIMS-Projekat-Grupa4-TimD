using InitialProject.Application.Injector;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _repository;
        public AccommodationRenovationService()
        {
            _repository = RepositoryInjector.Get<IAccommodationRenovationRepository>();
        }
        public List<AccommodationRenovation> GetAll()
        {
            return _repository.GetAll();
        }
        public void ScheduleRenovation(int id, DateTime start, DateTime end, string description)
        {
            _repository.ScheduleRenovation(id, start, end, description);
        }
    }
}
