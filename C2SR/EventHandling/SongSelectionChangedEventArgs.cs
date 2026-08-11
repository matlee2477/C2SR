using System.Collections;

namespace C2SR.EventHandling
{
    public class SongSelectionChangedEventArgs : EventArgs
    {
        public SongSelectionChangedEventArgs(IList selectedItems)
        {
            SelectedItems = selectedItems;
        }

        public IList SelectedItems { get; }
    }

    public delegate void SongSelectionChangedEventHandler(object sender, SongSelectionChangedEventArgs e);
}
