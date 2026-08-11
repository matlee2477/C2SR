using C2SR.Resources;
using C2SR.Services;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class TotalScoreToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            C2TopSongResult result = (C2TopSongResult)value;

            string rankName;
            if (result.IsUnranked)
            {
                rankName = Strings.Rank_Unranked;
            }
            else
            {
                rankName = C2TotalScoreService.Instance.GetRankFromTotalScore(result.TotalScore).Name;
            }

            return string.Format(Strings.MainWindow_TotalScoreText, result.TotalScore, rankName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
