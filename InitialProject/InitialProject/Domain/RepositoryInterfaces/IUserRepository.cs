using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User Update(User user);
        User GetById(int id);
    }
}
