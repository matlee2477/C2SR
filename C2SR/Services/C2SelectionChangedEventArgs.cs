using System.Collections;

namespace C2SR.Services
{
    class C2SelectionChangedEventArgs : EventArgs
    {
        public C2SelectionChangedEventArgs(IList selectedItems)
        {
            SelectedItems = selectedItems;
        }

        public IList SelectedItems { get; }
    }

    delegate void C2SelectionChangedEventHandler(object sender, C2SelectionChangedEventArgs e);
}
