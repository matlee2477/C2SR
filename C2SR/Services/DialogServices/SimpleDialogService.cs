
using System.Windows;

namespace C2SR.Services.DialogServices
{
    abstract class SimpleDialogService : ISimpleDialogService
    {
        public SimpleDialogService()
        {
            Owner = Application.Current.MainWindow;
        }

        // Properties
        public Window Owner { get; set; }

        // Methods
        public abstract void ShowDialog();
    }
}
