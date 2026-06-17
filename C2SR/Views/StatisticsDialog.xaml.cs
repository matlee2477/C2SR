using C2SR.Services;
using C2SR.Services.RegistryServices;
using C2SR.ViewModels;
using System.Windows;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for StatisticsDialog.xaml
    /// </summary>
    public partial class StatisticsDialog : Window
    {
        public StatisticsDialog(C2SongViewModel[] topSongs, decimal totalScore, bool isUnranked)
        {
            InitializeComponent();

            // Load window state
            using C2StatisticsRegistryService reg = new();
            Left = reg.WindowLeft;
            Top = reg.WindowTop;
            Width = reg.WindowWidth;
            Height = reg.WindowHeight;
            WindowState = reg.IsMaximized ? WindowState.Maximized : WindowState.Normal;

            // Load summary screen
            {

            }

            // Load rank information
            {
                itemsControl.ItemsSource = C2TotalScoreService.Instance.GetAllRanks();
            }
        }

        #region Event Handlers
        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save window state
            using C2StatisticsRegistryService reg = new();
            reg.WindowLeft = (int)Left;
            reg.WindowTop = (int)Top;
            reg.WindowWidth = (int)Width;
            reg.WindowHeight = (int)Height;
            reg.IsMaximized = WindowState == WindowState.Maximized;
        }

        #endregion
    }
}
