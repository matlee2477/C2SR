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

        // Fields
        C2Filter currentFilter;

        // Events
        public event C2SelectionChangedEventHandler? SelectionChanged;
        public event C2ApplyFiltersExecutedEventHandler? ApplyFiltersExecuted;

        // Methods
        public void RefreshListView() => listView.Items.Refresh();
        public void SelectAll() => listView.SelectAll();
        public void ExecuteApplyFilters() => ApplyFiltersExecuted?.Invoke(this, new ApplyFiltersExecutedEventArgs(currentFilter));

        // Event Handlers
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new C2SelectionChangedEventArgs(listView.SelectedItems));
        }

        private void SearchBar_SearchExecuted(object sender, SearchExecutedEventArgs e)
        {
            currentFilter = new()
            {
                SearchOption = e.SearchTarget,
                SearchTerm = e.SearchTerm,
                IsCaseSensitive = e.IsCaseSensitive,
                SortOption = currentFilter.SortOption,
                IsDescending = currentFilter.IsDescending,
                VersionFilter = currentFilter.VersionFilter,
                ChapterFilter = currentFilter.ChapterFilter,
                ChartTypeFilter = currentFilter.ChartTypeFilter,
                LevelFilter = currentFilter.LevelFilter,
                IsMMOnly = currentFilter.IsMMOnly,
                IsTP100Only = currentFilter.IsTP100Only,
                IsMxmOnly = currentFilter.IsMxmOnly
            };

            ExecuteApplyFilters();
        }

        private void FiltersPanel_FilterExecuted(object sender, FilterExecutedEventArgs e)
        {
            currentFilter = new()
            {
                SearchOption = currentFilter.SearchOption,
                SearchTerm = currentFilter.SearchTerm,
                IsCaseSensitive = currentFilter.IsCaseSensitive,
                SortOption = e.SortOption,
                IsDescending = e.IsDescending,
                VersionFilter = e.VersionFilter,
                ChapterFilter = e.ChapterFilter,
                ChartTypeFilter = e.ChartTypeFilter,
                LevelFilter = e.LevelFilter,
                IsMMOnly = e.IsMMOnly,
                IsTP100Only = e.IsTP100Only,
                IsMxmOnly = e.IsMxmOnly
            };

            ExecuteApplyFilters();
        }
    }
}
