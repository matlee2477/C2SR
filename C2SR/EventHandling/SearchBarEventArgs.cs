namespace C2SR.EventHandling
{
    public class SearchBarEventArgs : EventArgs
    {
        public SearchBarEventArgs(SearchOption searchTarget, string searchTerm, bool isCaseSensitive)
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

    public delegate void SearchBarEventHandler(object sender, SearchBarEventArgs e);
}
