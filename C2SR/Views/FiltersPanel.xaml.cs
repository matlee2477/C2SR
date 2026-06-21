using C2SR.EventHandling;
using C2SR.Stores;
using System.Windows;
using System.Windows.Controls;

namespace C2SR.Views
{
    /// <summary>
    /// Interaction logic for FiltersPanel.xaml
    /// </summary>
    public partial class FiltersPanel : UserControl
    {
        public FiltersPanel()
        {
            InitializeComponent();

            comboBox_FilterVersion.ItemsSource = DropdownItemStore.Instance.Versions;
            comboBox_FilterChapter.ItemsSource = DropdownItemStore.Instance.Chapters;
            comboBox_FilterChartType.ItemsSource = DropdownItemStore.Instance.ChartTypes;
            comboBox_FilterLevel.ItemsSource = DropdownItemStore.Instance.Levels;

            comboBox_FilterVersion.SelectedIndex = 0;
            comboBox_FilterChapter.SelectedIndex = 0;
            comboBox_FilterChartType.SelectedIndex = 0;
            comboBox_FilterLevel.SelectedIndex = 0;

            isFilterReady = true;
        }

        #region Fields
        readonly bool isFilterReady = false;

        public static readonly DependencyProperty SortOptionProperty = DependencyProperty.Register(
            nameof(SortOption),
            typeof(SortOption),
            typeof(FiltersPanel),
            new PropertyMetadata(SortOption.Default));

        public static readonly DependencyProperty IsDescendingProperty = DependencyProperty.Register(
            nameof(IsDescending),
            typeof(bool),
            typeof(FiltersPanel),
            new PropertyMetadata(false));

        public static readonly DependencyProperty VersionFilterProperty = DependencyProperty.Register(
            nameof(VersionFilter),
            typeof(string),
            typeof(FiltersPanel),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ChapterFilterProperty = DependencyProperty.Register(
            nameof(ChapterFilter),
            typeof(string),
            typeof(FiltersPanel),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ChartTypeFilterProperty = DependencyProperty.Register(
            nameof(ChartTypeFilter),
            typeof(string),
            typeof(FiltersPanel),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LevelFilterProperty = DependencyProperty.Register(
            nameof(LevelFilter),
            typeof(object),
            typeof(FiltersPanel),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsMMOnlyProperty = DependencyProperty.Register(
            nameof(IsMMOnly),
            typeof(bool),
            typeof(FiltersPanel),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsTP100OnlyProperty = DependencyProperty.Register(
            nameof(IsTP100Only),
            typeof(bool),
            typeof(FiltersPanel),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsMxmOnlyProperty = DependencyProperty.Register(
            nameof(IsMxmOnly),
            typeof(bool),
            typeof(FiltersPanel),
            new PropertyMetadata(false));

        #endregion

        #region Properties
        public SortOption SortOption
        {
            get => (SortOption)GetValue(SortOptionProperty);
            set => SetValue(SortOptionProperty, value);
        }

        public bool IsDescending
        {
            get => (bool)GetValue(IsDescendingProperty);
            set => SetValue(IsDescendingProperty, value);
        }

        public string VersionFilter
        {
            get => (string)GetValue(VersionFilterProperty);
            set => SetValue(VersionFilterProperty, value);
        }

        public string ChapterFilter
        {
            get => (string)GetValue(ChapterFilterProperty);
            set => SetValue(ChapterFilterProperty, value);
        }

        public string ChartTypeFilter
        {
            get => (string)GetValue(ChartTypeFilterProperty);
            set => SetValue(ChartTypeFilterProperty, value);
        }

        public object LevelFilter
        {
            get => GetValue(LevelFilterProperty);
            set => SetValue(LevelFilterProperty, value);
        }

        public bool IsMMOnly
        {
            get => (bool)GetValue(IsMMOnlyProperty);
            set => SetValue(IsMMOnlyProperty, value);
        }

        public bool IsTP100Only
        {
            get => (bool)GetValue(IsTP100OnlyProperty);
            set => SetValue(IsTP100OnlyProperty, value);
        }

        public bool IsMxmOnly
        {
            get => (bool)GetValue(IsMxmOnlyProperty);
            set => SetValue(IsMxmOnlyProperty, value);
        }

        #endregion

        // Events
        public event FilterExecutedEventHandler? FilterExecuted;

        #region Event handlers
        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if (isFilterReady)
            {
                FilterExecuted?.Invoke(this, new FilterExecutedEventArgs(SortOption, IsDescending,
                    comboBox_FilterVersion.SelectedIndex > 0 ? VersionFilter : null,
                    comboBox_FilterChapter.SelectedIndex > 0 ? ChapterFilter : null,
                    comboBox_FilterChartType.SelectedIndex > 0 ? ChartTypeFilter : null,
                    comboBox_FilterLevel.SelectedIndex > 0 ? (decimal)LevelFilter : null,
                    IsMMOnly, IsTP100Only, IsMxmOnly));
            }
        }

        private void comboBox_SearchOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDescending = SortOption switch
            {
                SortOption.Bpm or
                SortOption.Level or
                SortOption.LevelConstant or
                SortOption.Score => true,
                _ => false
            };

            FilterChanged(sender, e);
        }

        #endregion
    }
}
