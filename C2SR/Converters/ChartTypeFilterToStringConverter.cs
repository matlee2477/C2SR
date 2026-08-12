using C2SR.Resources;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class ChartTypeFilterToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string chartType && !string.IsNullOrEmpty(chartType))
            {
                return chartType;
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
