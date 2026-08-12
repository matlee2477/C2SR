using C2SR.EventHandling;
using C2SR.Models;
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
            comboBox_FilterLevel_Minimum.ItemsSource = DropdownItemStore.Instance.Levels;
            comboBox_FilterLevel_Maximum.ItemsSource = DropdownItemStore.Instance.Levels;

            ClearFilters();

            isFilterReady = true;
            FilterChanged(this, new());
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
            typeof(C2SongVersion),
            typeof(FiltersPanel),
            new PropertyMetadata());

        public static readonly DependencyProperty ChapterFilterProperty = DependencyProperty.Register(
            nameof(ChapterFilter),
            typeof(string),
            typeof(FiltersPanel),
            new PropertyMetadata());

        public static readonly DependencyProperty ChartTypeFilterProperty = DependencyProperty.Register(
            nameof(ChartTypeFilter),
            typeof(string),
            typeof(FiltersPanel),
            new PropertyMetadata());

        public static readonly DependencyProperty MinimumLevelFilterProperty = DependencyProperty.Register(
            nameof(MinimumLevelFilter),
            typeof(decimal),
            typeof(FiltersPanel),
            new PropertyMetadata(decimal.MinValue));

        public static readonly DependencyProperty MaximumLevelFilterProperty = DependencyProperty.Register(
            nameof(MaximumLevelFilter),
            typeof(decimal),
            typeof(FiltersPanel),
            new PropertyMetadata(decimal.MaxValue));

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

        public C2SongVersion VersionFilter
        {
            get => (C2SongVersion)GetValue(VersionFilterProperty);
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

        public decimal MinimumLevelFilter
        {
            get => (decimal)GetValue(MinimumLevelFilterProperty);
            set => SetValue(MinimumLevelFilterProperty, value);
        }

        public decimal MaximumLevelFilter
        {
            get => (decimal)GetValue(MaximumLevelFilterProperty);
            set => SetValue(MaximumLevelFilterProperty, value);
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
        public event FiltersPanelEventHandler? FilterExecuted;

        // Methods
        public void ClearFilters()
        {
            comboBox_FilterVersion.SelectedIndex = 0;
            comboBox_FilterChapter.SelectedIndex = 0;
            comboBox_FilterChartType.SelectedIndex = 0;
            comboBox_FilterLevel_Minimum.SelectedIndex = 0;
            comboBox_FilterLevel_Maximum.SelectedIndex = comboBox_FilterLevel_Maximum.Items.Count - 1;
            IsMMOnly = false;
            IsTP100Only = false;
            IsMxmOnly = false;
        }

        #region Event handlers
        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if (isFilterReady)
            {
                FilterExecuted?.Invoke(this, new(SortOption, IsDescending,
                    comboBox_FilterVersion.SelectedIndex > 0 ? VersionFilter : C2SongVersion.Empty,
                    comboBox_FilterChapter.SelectedIndex > 0 ? ChapterFilter : string.Empty, 
                    comboBox_FilterChartType.SelectedIndex > 0 ? ChartTypeFilter : string.Empty,
                    MinimumLevelFilter, MaximumLevelFilter,
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

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearFilters();
        }

        #endregion
    }
}
