using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.FileHandler
{
    public class UserFileHandler
    {
        private const string _usersFilePath = "../../../Resources/Data/users.csv";
        private readonly Serializer<User> _serializer;

        public UserFileHandler()
        {
            _serializer = new Serializer<User>();
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
