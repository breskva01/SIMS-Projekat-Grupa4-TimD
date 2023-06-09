using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public enum ComplexRequestStatus
    {
        OnHold,
        Invalid,
        Approved,
        PartiallyApproved
    }
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ComplexRequestStatus Status { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public List<int> TourRequestIDs { get; set; }

        public ComplexTourRequest()
        {
            Status = ComplexRequestStatus.OnHold;
            TourRequests = new List<TourRequest>();
            TourRequestIDs = new List<int>();
        }
        

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Status = (ComplexRequestStatus)Enum.Parse(typeof(ComplexRequestStatus), values[2]);
            List<string> TourRequestStringIDs = new List<string>(values[3].Split(','));
            TourRequestIDs = TourRequestStringIDs.ConvertAll(str => Convert.ToInt32(str));
        }

        public string[] ToCSV()
        {
            List<string> TourRequestStringIDs = TourRequestIDs.ConvertAll(str => str.ToString());
            string tourRequestStringIDs = string.Join(",", TourRequestStringIDs);
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Status.ToString(),
                tourRequestStringIDs
            };
            return csvValues;
        }
    }
}
