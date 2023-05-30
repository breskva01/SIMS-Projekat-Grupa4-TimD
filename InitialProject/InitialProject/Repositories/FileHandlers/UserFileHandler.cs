using InitialProject.Application.Serializer;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repositories.FileHandlers
{
    public class UserFileHandler
    {
        private const string _usersFilePath = "../../../Resources/Data/users.csv";
        private readonly UserSerializer _serializer;

        public UserFileHandler()
        {
            _serializer = new UserSerializer();
        }
        public List<User> Load()
        {
            return _serializer.FromCSV(_usersFilePath);
        }
        public void Save(List<User> users)
        {
            _serializer.ToCSV(_usersFilePath, users);
        }
    }
}
