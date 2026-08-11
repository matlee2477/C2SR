using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace C2SR.Converters
{
    class RankColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;

            if (color != Colors.Black)
            {
                return new SolidColorBrush(color);
            }

            // Create a gradient brush
            LinearGradientBrush grad = new();
            grad.GradientStops.Add(new(Colors.Red, 0.0));
            grad.GradientStops.Add(new(Colors.Orange, 0.1));
            grad.GradientStops.Add(new(Colors.Yellow, 0.3));
            grad.GradientStops.Add(new(Colors.LightGreen, 0.6));
            grad.GradientStops.Add(new(Colors.SkyBlue, 0.8));
            grad.GradientStops.Add(new(Colors.DeepSkyBlue, 1.0));

            return grad;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
