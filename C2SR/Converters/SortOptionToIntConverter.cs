using C2SR.EventHandling;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class SortOptionToIntConverter : IValueConverter
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
                0 => SortOption.Default,
                1 => SortOption.Name,
                2 => SortOption.Artist,
                3 => SortOption.Bpm,
                4 => SortOption.Version,
                5 => SortOption.ChartType,
                6 => SortOption.Level,
                7 => SortOption.LevelConstant,
                8 => SortOption.Score,
                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid integer value for SearchBarSearchOption.")
            };
        }
    }
}
