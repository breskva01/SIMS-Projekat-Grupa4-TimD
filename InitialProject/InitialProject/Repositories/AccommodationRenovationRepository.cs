using InitialProject.Application.Injector;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private readonly AccommodationRenovationFileHandler _fileHandler;
        private List<AccommodationRenovation> _renovations;
        private List<Accommodation> _accommodations;
        private IAccommodationRepository _accommodaitonRepository;
        public AccommodationRenovationRepository()
        {
            _fileHandler = new AccommodationRenovationFileHandler();
            _accommodaitonRepository = RepositoryInjector.Get<IAccommodationRepository>();
            _accommodations = _accommodaitonRepository.GetAll();
        }
        public List<AccommodationRenovation> GetAll()
        {
            return _renovations = _fileHandler.Load();
        }
        private int NextId()
        {
            if (_renovations.Count() == 0)
                return 1;
            return _renovations?.Max(r => r.Id) + 1 ?? 0;
        }

        public void ScheduleRenovation(int id, DateTime start, DateTime end, string description)
        {
            GetAll();
            Accommodation accommodation = _accommodations.Find(a => a.Id == id);
            AccommodationRenovation renovation = new AccommodationRenovation(NextId(), accommodation, start, end, description, end.AddYears(1), AppointmentStatus.Reserved);
            _renovations.Add(renovation);
            _fileHandler.Save(_renovations);
        }
        public List<AccommodationRenovation> GetAllAppointmentsByOwner(int id)
        {
            GetAll();
            List<AccommodationRenovation> renovations = new List<AccommodationRenovation>();
            foreach(AccommodationRenovation renovation in _renovations)
            {
                if(renovation.Accommodation.Owner.Id == id && !(renovation.Status == AppointmentStatus.Cancelled))
                {
                    renovations.Add(renovation);
                }
            }
            return renovations;
        }
        public void UpdateStatus(int id)
        {
            GetAll();
            AccommodationRenovation renovation = _renovations.Find(r => r.Id == id);
            AccommodationRenovation newRenovation = renovation;
            if (renovation.Status == AppointmentStatus.Reserved)
            {
                newRenovation.Status = AppointmentStatus.Finished;
                _renovations.Remove(renovation);
                _renovations.Add(newRenovation);
                _fileHandler.Save(_renovations);
            }
        }
        public void CancelAppointment(int id)
        {
            GetAll();
            AccommodationRenovation renovation = _renovations.Find(r => r.Id == id);
            AccommodationRenovation newRenovation = renovation;
            if (DateTime.Now <= renovation.Start.AddDays(-5))
            {
                newRenovation.Status = AppointmentStatus.Cancelled;
                _renovations.Remove(renovation);
                _renovations.Add(newRenovation);
                _fileHandler.Save(_renovations);
            }
        }

        public List<AccommodationRenovation> GetFilteredRenovations(int? accommodationId = null, 
            DateOnly? startDate = null, DateOnly? endDate = null)
        {
            var renovations = GetAll();

            if (accommodationId.HasValue)
                renovations = renovations.FindAll(r => r.Accommodation.Id == accommodationId.Value);

            if (startDate.HasValue)
                renovations = renovations.FindAll(r => DateOnly.FromDateTime(r.Start) >= startDate.Value);

            if (endDate.HasValue)
                renovations = renovations.FindAll(r => DateOnly.FromDateTime(r.End) <= endDate.Value);
            return renovations;
        }
    }
}
