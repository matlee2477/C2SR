using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2LevelConstantFontWeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal level = (decimal)values[0];
            decimal levelConstant = (decimal)values[1];
            decimal diff = levelConstant - level;
            return diff switch
            {
                >= 0.5M or <= -0.5M => FontWeights.Bold,
                _ => FontWeights.Normal
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotImplementedException();
        }
    }
}
