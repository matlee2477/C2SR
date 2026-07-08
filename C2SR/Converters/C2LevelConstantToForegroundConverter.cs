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
            decimal level = (decimal)values[0];
            decimal levelConstant = (decimal)values[1];
            if (C2SettingService.Instance.HighlightsBossSongs)
            {
                if (levelConstant >= BOSS_SONG_LEVEL_CONSTANT_THRESHOLD) return new SolidColorBrush(Colors.DarkViolet);
            }

            if (C2SettingService.Instance.HighlightsOutlyingLevelConstants)
            {
                decimal diff = levelConstant - level;
                return diff switch
                {
                    >= 0.3M => new SolidColorBrush(Colors.Red),
                    <= -0.3M => new SolidColorBrush(Colors.Blue),
                    _ => new SolidColorBrush(Colors.Black)
                };
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }

        // Constants
        const decimal BOSS_SONG_LEVEL_CONSTANT_THRESHOLD = 16.5M;
    }
}
