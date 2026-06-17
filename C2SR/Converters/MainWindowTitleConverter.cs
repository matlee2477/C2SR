using C2SR.Resources;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace C2SR.Converters
{
    class MainWindowTitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = (string)values[0];
            bool isSaved = (bool)values[1];

            StringBuilder sb = new();
            if (string.IsNullOrEmpty(fileName))
            {
                sb.Append("Untitled");
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(fileName));
            }
            if (!isSaved) sb.Append('*');
            sb.Append(" - ");
            sb.Append(Strings.Title);
            return sb.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
