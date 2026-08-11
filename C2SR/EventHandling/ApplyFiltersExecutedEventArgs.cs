namespace C2SR.EventHandling
{
    public class ApplyFiltersExecutedEventArgs : EventArgs
    {
        public ApplyFiltersExecutedEventArgs(C2Filter filter)
        {
            Filter = filter;
        }

        // Properties
        public C2Filter Filter { get; }
    }

    public readonly struct C2Filter
    {
        public SearchOption SearchOption { get; init; }
        public string SearchTerm { get; init; }
        public bool IsCaseSensitive { get; init; }
        public SortOption SortOption { get; init; }
        public bool IsDescending { get; init; }
        public string? VersionFilter { get; init; }
        public string? ChapterFilter { get; init; }
        public string? ChartTypeFilter { get; init; }
        public decimal MinimumLevelFilter { get; init; }
        public decimal MaximumLevelFilter { get; init; }
        public bool IsMMOnly { get; init; }
        public bool IsTP100Only { get; init; }
        public bool IsMxmOnly { get; init; }
    }

    public delegate void C2ApplyFiltersExecutedEventHandler(object sender, ApplyFiltersExecutedEventArgs e);
}
