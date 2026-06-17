using C2SR.Resources;
using C2SR.Services;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2TotalScoreToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal totalScore = (decimal)values[0];
            bool isUnranked = (bool)values[1];

            string rankName;
            if (isUnranked)
            {
                rankName = Strings.Rank_Unranked;
            }
            else
            {
                rankName = C2TotalScoreService.Instance.GetRankFromTotalScore(totalScore).Name;
            }

            return string.Format(Strings.MainWindow_TotalScoreText, totalScore.ToString("N3"), rankName);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
