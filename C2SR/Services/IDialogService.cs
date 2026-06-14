using System.Windows;

namespace C2SR.Services
{
    interface IDialogService
    {
        public Window Owner { get; set; }

        public MessageBoxResult QuerySaveChangesDialog();
        public bool ShowOpenFileDialog(out string fileName);
        public bool ShowSaveFileDialog(out string fileName);
        public void ShowOpenErrorDialog();
        public void ShowSaveErrorDialog();
    }
}
