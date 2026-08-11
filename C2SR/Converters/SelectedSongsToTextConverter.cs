using C2SR.Resources;
using C2SR.ViewModels;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class SelectedSongsToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int selectedCount = ((C2SongViewModel[])value).Length;
            return string.Format(Strings.MainWindow_StatusBarText_SongSelection, selectedCount);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
