﻿using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class KeyPoint : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Attraction { get; set; }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            Attraction = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                LocationId.ToString(),
                Attraction
            };
            return csvValues;
        }
    }
}
