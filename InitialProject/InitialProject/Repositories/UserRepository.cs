using InitialProject.Application.Serializer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repositories.FileHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

namespace InitialProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserFileHandler _fileHandler;
        /*private readonly IAccommodationRatingRepository _accommmodationRatingRepository;
        private List<AccommodationRating> _ratings;*/

        private List<User> _users;

        public UserRepository()
        {
            _fileHandler = new UserFileHandler();
            //_accommmodationRatingRepository = RepositoryStore.GetIAccommodationRatingRepository;
            _users = _fileHandler.Load();
        }

        public User GetByUsername(string username)
        {
            _users = _fileHandler.Load();
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public List<User> GetAll()
        {
            return _fileHandler.Load();
        }
        /*public void UpdateSuperOwnerStatus(int ownerId, double averageRating) 
        {
            User newOwner = new User();
            _users = _fileHandler.Load();
            _ratings = _accommmodationRatingRepository.GetByOwnerId(ownerId);
            User owner = _users.Find(o => o.Id == ownerId);
            int OwnerRatingsCount = 1;
            double totalAverageRating;
            AccommodationReservation accommodationReservation = new AccommodationReservation();

            foreach (AccommodationRating ar in _ratings)
            {
                    int[] rating = {ar.Location, ar.Hygiene, ar.Pleasantness, ar.Fairness, ar.Parking };
                    averageRating += rating.Average();
                    OwnerRatingsCount++;
            }

            totalAverageRating = averageRating / OwnerRatingsCount;
            if (averageRating >= 4.5 && OwnerRatingsCount >= 2)
                owner.SuperOwner = true;
            else
            {
                owner.SuperOwner = false;
            }
            newOwner = owner;
            _users.Remove(owner);
            _users.Add(newOwner);
            _fileHandler.Save(_users);
        }*/
    }
}
