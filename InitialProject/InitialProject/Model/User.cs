using InitialProject.Serializer;
using System;
using System.Windows.Documents;

namespace InitialProject.Model
{
    public enum UserType { Owner, Guest1, TourGuide, Guest2};
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public List<Rating> Ratings { get; set; }

        public User() { }

        public User(string username, string password, UserType userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, UserType.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            UserType = (UserType)Enum.Parse(typeof(UserType), values[3]);
        }
    }
}
