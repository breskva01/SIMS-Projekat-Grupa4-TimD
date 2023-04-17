using InitialProject.Application.Observer;
using InitialProject.Application.Stores;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Services
{
    public class UserService
    {
       // private readonly IUserRepository _repository;
       private readonly UserRepository _repository;
        public UserService()
        {
            //_repository = RepositoryStore.GetIUserRepository;
            _repository = new UserRepository();
        }
        public User GetByUsername(string username)
        {
            return _repository.GetByUsername(username);
        }
        public List<User> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
