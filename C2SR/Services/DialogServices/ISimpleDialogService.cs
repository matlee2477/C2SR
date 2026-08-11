using System.Windows;

namespace C2SR.Services.DialogServices
{
    interface ISimpleDialogService
    {
        public Window Owner { get; set; }
        public void ShowDialog();
    }
}
