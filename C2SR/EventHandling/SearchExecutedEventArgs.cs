namespace C2SR.EventHandling
{
    public class SearchExecutedEventArgs : EventArgs
    {
        public SearchExecutedEventArgs(SearchOption searchTarget, string searchTerm, bool isCaseSensitive)
        {
            SearchTarget = searchTarget;
            SearchTerm = searchTerm;
            IsCaseSensitive = isCaseSensitive;
        }

        // Properties
        public SearchOption SearchTarget { get; }
        public string SearchTerm { get; }
        public bool IsCaseSensitive { get; }
    }

    public enum SearchOption
    {
        Name,
        Artist,
    }

    public delegate void SearchExecutedEventHandler(object sender, SearchExecutedEventArgs e);
}
