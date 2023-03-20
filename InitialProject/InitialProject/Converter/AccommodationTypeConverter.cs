using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.Converter
{
    public class AccommodationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AccommodationType accommodationType)
            {
                switch (accommodationType)
                {
                    case AccommodationType.House:
                        return "Kuća";
                    case AccommodationType.Apartment:
                        return "Apartman";
                    case AccommodationType.Cottage:
                        return "Koliba";
                    default:
                        return "";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
