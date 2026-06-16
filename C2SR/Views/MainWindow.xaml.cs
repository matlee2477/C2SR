using C2SR.Resources;
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
        }

        // Properties
        public string[] ColumnHeaders
        {
            get
            {
                GridView gridView = (GridView)listView.View;
                return [.. gridView.Columns.Select(c => c.Header.ToString() ?? string.Empty)];
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
            sb.Append(" - ");
            sb.Append(Strings.Title);
            Title = sb.ToString();
        }

        // Event Handlers
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new C2SelectionChangedEventArgs(listView.SelectedItems));
        }
    }
}
