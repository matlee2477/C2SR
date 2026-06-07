using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Fields
        readonly MainWindowViewModel vm;

        // Methods
        public void SelectAll()
        {
            listView.SelectAll();
        }

        // Event Handlers
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.SelectedSongs = listView.SelectedItems.Cast<Cytus2SongViewModel>();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            vm.QuerySaveChanges(out bool cancel);
            e.Cancel = cancel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save registry
            var reg = RegistryLoader.Instance;
            reg.WindowLeft = (int)Left;
            reg.WindowTop = (int)Top;
            reg.WindowWidth = (int)Width;
            reg.WindowHeight = (int)Height;
            reg.IsMaximized = WindowState == WindowState.Maximized;
        }
    }
}
