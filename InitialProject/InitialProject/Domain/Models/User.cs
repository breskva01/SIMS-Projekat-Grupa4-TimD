using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace InitialProject.Domain.Models
{
    public enum UserType { Owner, Guest1, TourGuide, Guest2 };
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
        public double Rating { get; set; }
        public List<int> VouchersIds { get; set; }
        public int Age { get; set; }
        public bool SuperOwner { get; set; }

        public User() { }

        public User(string username, string password, UserType userType, string firstName, string lastName, string email, double rating, string phoneNumber, int age, bool superOwner)
        {
            Username = username;
            Password = password;
            Type = userType;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Rating = rating; 
            VouchersIds = new List<int>();
            Age = age;
            SuperOwner= superOwner;

        }

        public string[] ToCSV()
        {
            string voucherIds = "";
            foreach (int vId in VouchersIds)
            {
                voucherIds += vId.ToString() + ",";
            }
            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString(), FirstName, LastName,
                                   Email, Rating.ToString(), PhoneNumber , Age.ToString(), SuperOwner.ToString(), voucherIds};
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
            Rating = Convert.ToDouble(values[7]);
            PhoneNumber = values[8];
            Age = Convert.ToInt32(values[9]);
            SuperOwner = Convert.ToBoolean(values[10]);
            string vouchers = values[11];
            string[] splitVouchers = vouchers.Split(',');
            splitVouchers = splitVouchers.SkipLast(1).ToArray();
            VouchersIds = new List<int>();
            foreach (string voucherId in splitVouchers)
            {
                VouchersIds.Add(Convert.ToInt32(voucherId));
            }
        }
    }
}
