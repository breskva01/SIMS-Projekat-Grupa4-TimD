using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.WPF.Converters
{
    public class SecurePasswordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SecureString secureString)
            {
                IntPtr bstr = IntPtr.Zero;
                try
                {
                    bstr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
                    return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(bstr);
                }
                finally
                {
                    if (bstr != IntPtr.Zero)
                    {
                        System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(bstr);
                    }
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string password)
            {
                var securePassword = new SecureString();
                foreach (var c in password)
                {
                    securePassword.AppendChar(c);
                }

                return securePassword;
            }

            return new SecureString();
        }
    }
}
