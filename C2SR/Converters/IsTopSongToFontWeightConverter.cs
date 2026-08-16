using C2SR.Services;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace C2SR.Converters
{
    class IsTopSongToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (SettingService.Instance.HighlightsTopSongs)
            {
                bool isTopSong = (bool)value;
                return isTopSong ? FontWeights.Bold : FontWeights.Normal;
            }
            else
            {
                return FontWeights.Normal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
