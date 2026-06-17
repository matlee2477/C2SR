using System.Collections;

namespace C2SR.EventHandling
{
    public class C2SelectionChangedEventArgs : EventArgs
    {
        public C2SelectionChangedEventArgs(IList selectedItems)
        {
            SelectedItems = selectedItems;
        }

        public IList SelectedItems { get; }
    }

    public delegate void C2SelectionChangedEventHandler(object sender, C2SelectionChangedEventArgs e);
}
