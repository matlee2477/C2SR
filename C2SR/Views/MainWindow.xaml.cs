using C2SR.Services;
using System.Text;
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

            // Load registry
            {
                using C2RegistryService reg = new();
                Left = reg.WindowLeft;
                Top = reg.WindowTop;
                Width = reg.WindowWidth;
                Height = reg.WindowHeight;
                WindowState = reg.IsMaximized ? WindowState.Maximized : WindowState.Normal;
            }
        }

        // Events
        internal event C2SelectionChangedEventHandler? SelectionChanged;

        // Methods
        public void SelectAll() => listView.SelectAll();

        public void HandleChangeTitleRequest(string fileName, bool isSaved)
        {
            StringBuilder sb = new();
            if (string.IsNullOrEmpty(fileName))
            {
                sb.Append("Untitled");
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(fileName));
            }
            if (!isSaved) sb.Append('*');
            sb.Append(" - Cytus II Skill Rate");
            Title = sb.ToString();
        }

        // Event Handlers
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new C2SelectionChangedEventArgs(listView.SelectedItems));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save registry
            using C2RegistryService reg = new();
            reg.WindowLeft = (int)Left;
            reg.WindowTop = (int)Top;
            reg.WindowWidth = (int)Width;
            reg.WindowHeight = (int)Height;
            reg.IsMaximized = WindowState == WindowState.Maximized;
        }
    }
}
