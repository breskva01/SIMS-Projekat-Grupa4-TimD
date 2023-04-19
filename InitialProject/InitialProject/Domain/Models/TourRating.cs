﻿using InitialProject.Application.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class TourRating : ISerializable
    {
        public int Id { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int TourInteresting { get; set; }
        public int TourInformative { get; set; }
        public int TourContent { get; set; }
        public string Comment { get; set; }
        public List<string> PictureURLs { get; set; }
        public bool IsValid { get; set; }

        public TourRating()
        {
            Comment = string.Empty;
            PictureURLs = new List<string>();
            IsValid = true;
        }

        public TourRating(int guideKnowledge, int guideLanguage, int tourInteresting, int tourInformative, int tourContent, string comment, List<string> pictureURLs)
        {
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            TourInteresting = tourInteresting;
            TourInformative = tourInformative;
            TourContent = tourContent;
            Comment = comment;
            PictureURLs = pictureURLs;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuideKnowledge = int.Parse(values[1]);
            GuideLanguage = int.Parse(values[2]);
            TourInteresting = int.Parse(values[3]);
            TourInformative = int.Parse(values[4]);
            TourContent = int.Parse(values[5]);
            Comment = values[6];
            IsValid = Convert.ToBoolean(values[7]);
            PictureURLs = new List<string>(values[8].Split(','));
        }

        public string[] ToCSV()
        {
            string pictureURLs = string.Join(",", PictureURLs);
            string[] csvValues =
            {
                Id.ToString(),
                GuideLanguage.ToString(),
                GuideKnowledge.ToString(),
                TourInteresting.ToString(),
                TourInformative.ToString(),
                TourContent.ToString(),
                Comment,
                IsValid.ToString(),
                pictureURLs
            };
            return csvValues;
        }
    }
}
