using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Models
{
    public class TourFilterSort
    {
        public string FilterCountry;
        public string FilterCity;
        public int FilterMinDuration;
        public int FilterMaxDuration;
        public GuideLanguage FilterLanguage;
        public int FilterNumberOfGuests;

        public bool SortCountry;
        public bool SortCity;
        public bool SortDuration;
        public bool SortLanguage;
        public bool SortSpaces;


        public TourFilterSort(string filterCountry = "", string filterCity = "", int filterMinDuration = 0, int filterMaxDuration = 0, GuideLanguage filterLanguage = GuideLanguage.All, int filterNumberOfGuests = 0, bool sortCountry = false, bool sortCity = false, bool sortDuration = false, bool sortLanguage = false, bool sortSpaces = false)
        {
            FilterCountry = filterCountry;
            FilterCity = filterCity;
            FilterMinDuration = filterMinDuration;
            FilterMaxDuration = filterMaxDuration;
            FilterLanguage = filterLanguage;
            FilterNumberOfGuests = filterNumberOfGuests;
            SortCountry = sortCountry;
            SortCity = sortCity;
            SortDuration = sortDuration;
            SortLanguage = sortLanguage;
            SortSpaces = sortSpaces;
        }
    }
}
