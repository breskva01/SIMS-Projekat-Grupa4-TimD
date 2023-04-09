using InitialProject.FileHandler;
using InitialProject.Observer;
using InitialProject.Serializer;
using InitialProject.Storage;
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
        private readonly Storage<GuestRating> _storage;
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
        public void AddGuestRating(int id, GuestRating guestRating)
        {
            List<GuestRating> guestRatings = new List<GuestRating>();
            _users = _fileHandler.Load();
            foreach (User u in _users)
            {
                if (id == u.Id)
                {
                    guestRatings.Add(guestRating);
                    u.Ratings = guestRatings;
                }   
            }
        }
    }
}
