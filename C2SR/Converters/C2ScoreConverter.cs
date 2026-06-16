using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2ScoreConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal levelConstant = (decimal)values[0];
            bool isMM = (bool)values[1];
            decimal tp = (decimal)values[2];
            return GetScore(levelConstant, isMM, tp);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotImplementedException();
        }

        public static decimal GetScore(decimal levelConstant, bool isMM, decimal tp)
        {
            decimal rate = tp * levelConstant / 100;
            if (isMM) rate += 0.3m;
            if (tp == 100) rate += 0.2m;
            return rate;
        }
    }
}
