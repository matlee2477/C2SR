using C2SR.Resources;
using System.Windows;

namespace C2SR.Services.DialogServices
{
    class OpenErrorDialogService : SimpleDialogService
    {
        public OpenErrorDialogService() : base() { }

        // Methods
        public override void ShowDialog()
        {
            MessageBox.Show(Owner, Strings.MessageBox_Error_Load, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
