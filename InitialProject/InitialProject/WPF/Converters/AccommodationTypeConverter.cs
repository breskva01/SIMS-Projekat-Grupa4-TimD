using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.WPF.Converters
{
    public class AccommodationTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AccommodationType accommodationType)
                if (TranslationSource.Instance.CurrentCulture.Name == "sr-Latn")
                {
                    switch (accommodationType)
                    {
                        case AccommodationType.House:
                            return "Kuća";
                        case AccommodationType.Apartment:
                            return "Apartman";
                        case AccommodationType.Cottage:
                            return "Koliba";
                        case AccommodationType.Everything:
                            return "Sve";
                        default:
                            return "";
                    }
                }
                else
                {
                    return accommodationType;
                }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
