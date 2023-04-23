using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.WPF.Converters
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
                if (memberInfo != null)
                {
                    var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttribute != null)
                    {
                        return displayAttribute.GetName();
                    }
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
