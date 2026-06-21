using C2SR.EventHandling;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class SearchOptionToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            return intValue switch
            {
                0 => SearchOption.Name,
                1 => SearchOption.Artist,
                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid integer value for SearchBarSearchOption.")
            };
        }
    }
}
