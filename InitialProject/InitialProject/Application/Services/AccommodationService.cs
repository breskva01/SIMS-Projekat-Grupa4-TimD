using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _repository;
        public AccommodationService()
        {
            _repository = RepositoryInjector.Get<IAccommodationRepository>();
        }
        public List<Accommodation> GetAll()
        {
            return _repository.GetAll();
        }
        public List<Accommodation> GetFiltered(string keyWords, AccommodationType type, int guestNumber, int numberOfDays)
        {
            return _repository.GetFiltered(keyWords, type, guestNumber, numberOfDays);
        }
        public List<Accommodation> Sort(ObservableCollection<Accommodation> accommodations, string criterion)
        {
            return _repository.Sort(new List<Accommodation>(accommodations), criterion);
        }
        public void RegisterAccommodation(string name, string country, string city, string address, AccommodationType type, int maximumGuests,
            int minimumDays, int minimumCancelationNotice, string pictureURL, User user)
        {
            List<string> pictureURLs = new List<string>
            {
                pictureURL
            };
            _repository.Add(name, country, city, address, type, maximumGuests, minimumDays, minimumCancelationNotice, pictureURLs, user);
        }
    }
}
