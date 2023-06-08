using InitialProject.Application.Serializer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repositories.FileHandlers;
using System;
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

        public List<Guest1> GetAllGuest1s()
        {
            GetAll();
            return _users.OfType<Guest1>().ToList();
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

        public bool IsEligibleForFreeVoucher(Guest2 guest)
        {
            if(guest.FreeVoucherProgress == 0)
            {
                guest.FreeVoucherProgressLimit = DateTime.UtcNow.AddYears(1);
            }

            if(DateTime.UtcNow.CompareTo(guest.FreeVoucherProgressLimit) > 0)
            {
                guest.FreeVoucherProgress = 1;
                guest.FreeVoucherProgressLimit = DateTime.UtcNow.AddYears(1);
                Update(guest);
                return false;
            }
            else
            {
                guest.FreeVoucherProgress++;
                if(guest.FreeVoucherProgress == 5)
                {
                    guest.FreeVoucherProgress = 0;
                    Update(guest);
                    return true;
                }
                Update(guest);
                return false;
            }

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
