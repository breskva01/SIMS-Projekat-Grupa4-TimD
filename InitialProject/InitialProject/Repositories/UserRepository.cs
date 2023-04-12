using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using InitialProject.Repositories.FileHandlers;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class UserRepository
    {
        private readonly UserFileHandler _fileHandler;

        private List<User> _users;

        public UserRepository()
        {
            _fileHandler = new UserFileHandler();
            _users = _fileHandler.Load();
        }

        public User GetByUsername(string username)
        {
            _users = _fileHandler.Load();
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}
