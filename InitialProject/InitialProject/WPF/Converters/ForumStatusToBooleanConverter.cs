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
    public class ForumStatusToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ForumStatus status)
            {
                return status == ForumStatus.Open;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOpen)
            {
                return isOpen ? ForumStatus.Open : ForumStatus.Closed;
            }

            return ForumStatus.Closed;
        }
    }
}
