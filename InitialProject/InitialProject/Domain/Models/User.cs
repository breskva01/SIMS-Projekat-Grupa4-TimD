using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace InitialProject.Domain.Models
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }

        public User() { }
        public virtual string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, FirstName, LastName,
                                   Email, PhoneNumber , Age.ToString() };
            return csvValues;
        }

        public virtual void FromCSV(string[] values)
        {
            
            Id = Convert.ToInt32(values[1]);
            Username = values[2];
            Password = values[3];
            FirstName = values[4];
            LastName = values[5];
            Email = values[6];
            PhoneNumber = values[7];
            Age = Convert.ToInt32(values[8]);
        }
    }
}
