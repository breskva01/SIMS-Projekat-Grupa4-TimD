using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace InitialProject.Model
{
    public enum UserType { Owner, Guest1, TourGuide, Guest2};
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<GuestRating> Ratings { get; set; }

        public User() { }

        public User(string username, string password, UserType userType, string firstName, string lastName, string email, string phoneNumber)
        {
            Username = username;
            Password = password;
            Type = userType;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Ratings = new List<GuestRating>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString(), FirstName, LastName,
                                   Email, PhoneNumber };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Type = (UserType)Enum.Parse(typeof(UserType), values[3]);
            FirstName = values[4];
            LastName = values[5];
            Email = values[6];
            PhoneNumber = values[7];
        }
    }
}
