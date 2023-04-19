﻿using InitialProject.Application.Serializer;
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
