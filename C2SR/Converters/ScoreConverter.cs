using System.Globalization;
using System.Windows.Data;
using static C2SR.App.Constants;

namespace C2SR.Converters
{
    class ScoreConverter : IMultiValueConverter
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
            throw new NotSupportedException();
        }

        public static decimal GetScore(decimal levelConstant, bool isMM, decimal tp)
        {
            decimal score = levelConstant * (tp / 100) * (tp / 100);
            if (isMM) score += SCORE_BONUS_MM;
            if (tp == 100) score += SCORE_BONUS_TP100;
            return score;
        }
    }
}
