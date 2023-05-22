using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class UserNotification : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }


        public UserNotification()
        {
            Message = string.Empty;
            Time = DateTime.Now;
        }

        /*
        public UserNotification(int id, int userId, string message, DateTime time)
        {
            Id = id;
            UserId = userId;
            Message = message;
            Time = time;
        }
        */

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Message = values[2];
            Time = DateTime.ParseExact(values[3], "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Message,
                Time.ToString("dd.MM.yyyy. HH:mm:ss"),
            };
            return csvValues;
        }
    }
}
