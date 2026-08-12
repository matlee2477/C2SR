using C2SR.Resources;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class ChapterFilterToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string chapter && !string.IsNullOrEmpty(chapter))
            {
                return chapter;
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
