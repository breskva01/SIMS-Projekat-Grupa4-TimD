using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Models.DAO;
using InitialProject.Domain.Models;

namespace InitialProject.Controller
{
    public class UserController
    {
        private readonly UserDAO _userDAO;

        public UserController()
        {
            _userDAO = new UserDAO();
        }
        public User GetByUsername(string username)
        {
            return _userDAO.GetByUsername(username);
        }
        public List<User> GetUsers()
        {
            return _userDAO.GetUsers();
        }
        /*public void AddGuestRating(int id, GuestRating guestRating)
        {
            _userDAO.AddGuestRating(id, guestRating);
        }*/
    }
}
