using C2SR.EventHandling;
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

        // Events
        public event C2SelectionChangedEventHandler? SelectionChanged;

        // Methods
        public void RefreshListView() => listView.Items.Refresh();
        public void SelectAll() => listView.SelectAll();

        // Event Handlers
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new C2SelectionChangedEventArgs(listView.SelectedItems));
        }
    }
}
