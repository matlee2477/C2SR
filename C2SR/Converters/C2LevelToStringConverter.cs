using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2LevelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal level)
            {
                decimal levelFloor = Math.Floor(level);

                StringBuilder sb = new();
                sb.Append(levelFloor.ToString("N0"));
                if (levelFloor != level) sb.Append('+');
                return sb.ToString();
            }
            else
            {
                return (string)value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
