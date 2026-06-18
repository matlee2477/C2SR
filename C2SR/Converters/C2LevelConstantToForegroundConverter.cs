using C2SR.Services;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace C2SR.Converters
{
    class C2LevelConstantToForegroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (C2SettingService.Instance.HighlightsOutlyingLevelConstants)
            {
                decimal level = (decimal)values[0];
                decimal levelConstant = (decimal)values[1];
                decimal diff = levelConstant - level;
                return diff switch
                {
                    >= 0.3M => new SolidColorBrush(Colors.Red),
                    <= -0.3M => new SolidColorBrush(Colors.Blue),
                    _ => new SolidColorBrush(Colors.Black)
                };
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
