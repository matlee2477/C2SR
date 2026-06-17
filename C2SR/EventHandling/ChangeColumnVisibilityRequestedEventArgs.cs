namespace C2SR.EventHandling
{
    public class ChangeColumnVisibilityRequestedEventArgs
    {
        public ChangeColumnVisibilityRequestedEventArgs(bool[] columnVisibililties)
        {
            ColumnVisibililties = columnVisibililties;
        }

        // Properties
        public bool[] ColumnVisibililties { get; }
    }

    public delegate void ChangeColumnVisibilityRequestedEventHandler(object sender, ChangeColumnVisibilityRequestedEventArgs e);
}
