using C2SR.Models;

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
        public C2SongVersion VersionFilter { get; init; }
        public string ChapterFilter { get; init; }
        public string ChartTypeFilter { get; init; }
        public decimal MinimumLevelFilter { get; init; }
        public decimal MaximumLevelFilter { get; init; }
        public bool IsMMOnly { get; init; }
        public bool IsTP100Only { get; init; }
        public bool IsMxmOnly { get; init; }

        public static readonly C2Filter Default = new()
        {
            SearchOption = SearchOption.Name,
            SearchTerm = string.Empty,
            IsCaseSensitive = false,
            SortOption = SortOption.Default,
            IsDescending = false,
            VersionFilter = C2SongVersion.Empty,
            ChapterFilter = string.Empty,
            ChartTypeFilter = string.Empty,
            MinimumLevelFilter = decimal.MinValue,
            MaximumLevelFilter = decimal.MaxValue,
            IsMMOnly = false,
            IsTP100Only = false,
            IsMxmOnly = false
        };
    }

    public delegate void C2ApplyFiltersExecutedEventHandler(object sender, ApplyFiltersExecutedEventArgs e);
}
