using System.Windows.Data;

namespace C2SR.Converters
{
    class C2TPConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal tp = (decimal)value;
            return tp.ToString("N2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string? str = value as string;
            if (decimal.TryParse(str, out decimal tp))
            {
                if (tp < 0) tp = 0;
                if (tp > 100) tp = 100;
                return tp;
            }
            else
            {
                return 0;
            }
        }
    }
}
