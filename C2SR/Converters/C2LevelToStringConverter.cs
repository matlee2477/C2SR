using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2LevelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal level = (decimal)value;
            decimal levelFloor = Math.Floor(level);

            StringBuilder sb = new();
            sb.Append(levelFloor.ToString("N0"));
            if (levelFloor != level) sb.Append('+');
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
