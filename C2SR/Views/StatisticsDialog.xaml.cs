using C2SR.Converters;
using C2SR.Resources;
using C2SR.Services;
using C2SR.Services.RegistryServices;
using C2SR.ViewModels;
using System.Windows;
using System.Windows.Media;
using static C2SR.App.Constants;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for StatisticsDialog.xaml
    /// </summary>
    public partial class StatisticsDialog : Window
    {
        public StatisticsDialog(IEnumerable<C2SongViewModel> songs)
        {
            InitializeComponent();

            // Load window state
            using StatisticsRegistryService reg = new();
            Left = reg.WindowLeft;
            Top = reg.WindowTop;
            Width = reg.WindowWidth;
            Height = reg.WindowHeight;
            WindowState = reg.IsMaximized ? WindowState.Maximized : WindowState.Normal;

            // Load summary screen
            {
                var result = C2TotalScoreService.GetTopSongs(songs);
                var count = songs.Count();
                var topCount = result.TopSongCount;
                var mmCount = songs.Count(s => s.IsMM);
                var tp100Count = songs.Count(s => s.TP == 100);
                var mxmCount = songs.Count(s => s.IsMxm);

                var rank = C2TotalScoreService.Instance.GetRankFromTotalScore(result.TotalScore);
                if (!result.IsUnranked)
                {
                    RankColorToBrushConverter conv = new();
                    stackPanel_Rank.Background = (Brush)conv.Convert(rank.Color, null!, null!, null!);
                    textBlock_RankName.Text = rank.Name;
                }

                textBlock_TotalScore.Text = string.Format(Strings.StatisticsDialog_Summary_TotalScore, result.TotalScore);
                textBlock_EvaluatedCount.Text = string.Format(Strings.StatisticsDialog_Summary_EvaluatedCount, topCount, TOTAL_SCORE_SONG_COUNT);
                textBlock_MMCount.Text = string.Format(Strings.StatisticsDialog_Summary_MMCount, mmCount, count);
                textBlock_TP100Count.Text = string.Format(Strings.StatisticsDialog_Summary_TP100Count, tp100Count, count);
                textBlock_MxmCount.Text = string.Format(Strings.StatisticsDialog_Summary_MxmCount, mxmCount, count);
                groupBox_TopSongs.Header = string.Format(Strings.StatisticsDialog_TopSongs, TOTAL_SCORE_SONG_COUNT);

                listView.ItemsSource = result.TopSongs;
            }

            // Load rank information
            itemsControl.ItemsSource = C2TotalScoreService.Instance.GetAllRanks();
        }

        #region Event Handlers
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save window state
            using StatisticsRegistryService reg = new();
            reg.WindowLeft = (int)Left;
            reg.WindowTop = (int)Top;
            reg.WindowWidth = (int)Width;
            reg.WindowHeight = (int)Height;
            reg.IsMaximized = WindowState == WindowState.Maximized;
        }

        #endregion
    }
}
