using InitialProject.FileHandler;
using InitialProject.Observer;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model.DAO
{
    internal class UserDAO
    {
        private readonly UserFileHandler _fileHandler;
        private List<User> _users;
        public UserDAO()
        {
            _fileHandler = new UserFileHandler();
            _users = _fileHandler.Load();
        }
        public User GetByUsername(string username)
        {
            _users = _fileHandler.Load();
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public List<User> GetUsers() {
            return _fileHandler.Load();
        }
    }
}
