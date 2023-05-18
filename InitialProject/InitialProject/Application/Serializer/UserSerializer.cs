using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Serializer
{
    public class UserSerializer
    {
        private const char Delimiter = '|';

        public void ToCSV(string fileName, List<User> users)
        {
            StringBuilder csv = new StringBuilder();

            foreach (User user in users)
            {
                string line = string.Join(Delimiter.ToString(), user.ToCSV());
                csv.AppendLine(line);
            }

            File.WriteAllText(fileName, csv.ToString());

        }

        public List<User> FromCSV(string fileName)
        {
            List<User> users = new List<User>();

            foreach (string line in File.ReadLines(fileName, Encoding.UTF8))
            {
                string[] csvValues = line.Split(Delimiter);
                User user;
                switch (csvValues[0])
                {
                    case "Guest1":
                        user = new Guest1();
                        break;
                    case "Guest2":
                        user = new Guest2();
                        break;
                    case "Owner":
                        user = new Owner();
                        break;
                    default:
                        user = new TourGuide();
                        break;
                }
                user.FromCSV(csvValues);
                users.Add(user);
            }

            return users;
        }
    }
}
