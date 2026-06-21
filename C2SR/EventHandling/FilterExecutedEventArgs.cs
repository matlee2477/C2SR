namespace C2SR.EventHandling
{
    public class FilterExecutedEventArgs : EventArgs
    {
        public FilterExecutedEventArgs(SortOption sortOption, bool isDescending,
            string? versionFilter, string? chapterFilter, string? chartTypeFilter, decimal? levelFilter, bool isMMOnly, bool isTP100Only, bool isMxmOnly)
        {
            SortOption = sortOption;
            IsDescending = isDescending;
            VersionFilter = versionFilter;
            ChapterFilter = chapterFilter;
            ChartTypeFilter = chartTypeFilter;
            LevelFilter = levelFilter;
            IsMMOnly = isMMOnly;
            IsTP100Only = isTP100Only;
            IsMxmOnly = isMxmOnly;
        }

        // Properties
        public SortOption SortOption { get; }
        public bool IsDescending { get; }
        public string? VersionFilter { get; }
        public string? ChapterFilter { get; }
        public string? ChartTypeFilter { get; }
        public decimal? LevelFilter { get; }
        public bool IsMMOnly { get; }
        public bool IsTP100Only { get; }
        public bool IsMxmOnly { get; }
    }

    public enum SortOption
    {
        Default,
        Name,
        Artist,
        Bpm,
        Version,
        ChartType,
        Level,
        LevelConstant,
        Score,
    }

    public delegate void FilterExecutedEventHandler(object sender, FilterExecutedEventArgs e);
}
