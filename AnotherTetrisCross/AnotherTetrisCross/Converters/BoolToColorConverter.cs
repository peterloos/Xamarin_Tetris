using System;
using System.Globalization;

using Xamarin.Forms;

namespace AnotherTetrisCross.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            bool b = (bool)value;

            return (b) ? Color.Red : Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
