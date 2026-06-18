using C2SR.Resources;
using C2SR.Services;
using System.Globalization;
using System.Windows.Data;

namespace C2SR.Converters
{
    class C2RankCriterionToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal criterion = (decimal)values[0];
            string rankName = (string)values[1];

            if (rankName == C2TotalScoreService.Instance.GetAllRanks().First().Name)
            {
                return criterion.ToString("N3");
            }
            else
            {
                return string.Format(Strings.StatisticsDialog_Ranks_Criterion, criterion.ToString("N3"));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not a two-way binding
            throw new NotSupportedException();
        }
    }
}
