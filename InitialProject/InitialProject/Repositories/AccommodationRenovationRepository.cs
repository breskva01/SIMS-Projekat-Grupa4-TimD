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
            AccommodationRenovation renovation = new AccommodationRenovation(NextId(), accommodation, start, end, description, end.AddYears(1));
            _renovations.Add(renovation);
            _fileHandler.Save(_renovations);
        }
    }
}
