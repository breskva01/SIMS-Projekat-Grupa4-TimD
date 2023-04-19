using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories.FileHandlers;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserFileHandler _fileHandler;

        private List<User> _users;

        public UserRepository()
        {
            _fileHandler = new UserFileHandler();
            _users = _fileHandler.Load();
        }

        public List<User> GetAll()
        {
            return _fileHandler.Load();
        }
        public User GetById(int id)
        {
            _users = _fileHandler.Load();
            return _users.Find(u => u.Id == id);
        }
        public User GetByUsername(string username)
        {
            _users = _fileHandler.Load();
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public User Update(User user)
        {
            _users = _fileHandler.Load();
            User updated = _users.Find(t => t.Id == user.Id);
            _users.Remove(updated);
            _users.Add(user);
            _fileHandler.Save(_users);
            return user;
        }
        
    }
}
