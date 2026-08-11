using C2SR.Resources;
using System.Windows;

namespace C2SR.Services.DialogServices
{
    class SaveErrorDialogService : SimpleDialogService
    {
        public SaveErrorDialogService() : base() { }

        // Methods
        public override void ShowDialog()
        {
            MessageBox.Show(Owner, Strings.MessageBox_Error_Save, Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
