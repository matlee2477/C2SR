namespace C2SR.Services
{
    class ChangeColumnVisibilityRequestedEventArgs
    {
        public ChangeColumnVisibilityRequestedEventArgs(bool[] columnVisibililties)
        {
            ColumnVisibililties = columnVisibililties;
        }

        // Properties
        public bool[] ColumnVisibililties { get; }
    }

    delegate void ChangeColumnVisibilityRequestedEventHandler(object sender, ChangeColumnVisibilityRequestedEventArgs e);
}
