using C2SR.Models;
using C2SR.Resources;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class VersionFilterToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is C2SongVersion level && level != C2SongVersion.Empty)
            {
                return level.ToString();
            }
            else
            {
                return Strings.MainWindow_Filters_All;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
