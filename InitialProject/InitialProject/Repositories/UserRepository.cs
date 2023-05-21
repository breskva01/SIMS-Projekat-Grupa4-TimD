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
        }

        public List<User> GetAll()
        {
            return _users = _fileHandler.Load();
        }
        public User GetById(int id)
        {
            GetAll();
            return _users.Find(u => u.Id == id);
        }
        public User GetByUsername(string username)
        {
            GetAll();
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public User Update(User user)
        {
            GetAll();
            User updated = _users.Find(t => t.Id == user.Id);
            _users.Remove(updated);
            _users.Add(user);
            _fileHandler.Save(_users);
            return user;
        }
    }
}
