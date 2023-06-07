﻿using InitialProject.Application.Injector;
using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService()
        {
            _repository = RepositoryInjector.Get<IUserRepository>();
        }
        public User GetByUsername(string username)
        {
            return _repository.GetByUsername(username);
        }
        public List<User> GetAll()
        {
            return _repository.GetAll();
        }
        public User Update(User user)
        {
            return _repository.Update(user);
        }
        public User GetById(int id)
        {
            return _repository.GetById(id);
        }
    }
}
