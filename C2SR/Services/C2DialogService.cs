using C2SR.Views;
using Microsoft.Win32;
using System.Windows;

namespace C2SR.Services
{
    class C2DialogService : IDialogService
    {
        public C2DialogService(Window owner)
        {
            Owner = owner;
        }

        // Properties
        public Window Owner { get; set; }

        // Methods
        public MessageBoxResult QuerySaveChangesDialog() => MessageBox.Show(Owner, MSG_SAVE_CHANGES, TITLE, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

        public bool ShowOpenFileDialog(out string fileName)
        {
            OpenFileDialog dialog = new()
            {
                Title = TITLE_OPEN,
                Filter = FILTER,
                DefaultExt = DEF_EXT,
                AddExtension = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(Owner) == true)
            {
                fileName = dialog.FileName;
                return true;
            }
            else
            {
                fileName = null!;
                return false;
            }
        }

        public bool ShowSaveFileDialog(out string fileName)
        {
            SaveFileDialog dialog = new()
            {
                Title = TITLE_SAVE,
                Filter = FILTER,
                DefaultExt = DEF_EXT,
                AddExtension = true,
                OverwritePrompt = true
            };

            if (dialog.ShowDialog(Owner) == true)
            {
                fileName = dialog.FileName;
                return true;
            }
            else
            {
                fileName = null!;
                return false;
            }
        }

        public void ShowOpenErrorDialog() => MessageBox.Show(Owner, MSG_OPEN_ERROR, TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ShowSaveErrorDialog() => MessageBox.Show(Owner, MSG_SAVE_ERROR, TITLE, MessageBoxButton.OK, MessageBoxImage.Error);

        public SetValueDialogResult ShowSetValueDialog()
        {
            SetValueDialog dialog = new() { Owner = Owner };
            if (dialog.ShowDialog() == true)
            {
                return new SetValueDialogResult
                {
                    DialogResult = true,
                    SetsMM = dialog.SetsMM,
                    SetsTP = dialog.SetsTP,
                    SetsMxm = dialog.SetsMxm,
                    IsMM = dialog.IsMM,
                    TP = dialog.TPValue,
                    IsMxm = dialog.IsMxm
                };
            }
            else
            {
                return new SetValueDialogResult { DialogResult = false };
            }
        }

        public void ShowAboutDialog()
        {
            AboutDialog dialog = new() { Owner = Owner };
            dialog.ShowDialog();
        }

        // Constants
        const string TITLE = "Cytus II Rating";
        const string MSG_SAVE_CHANGES = "Save changes to the current document? All unsaved data will be discarded.";
        const string TITLE_OPEN = "Open";
        const string TITLE_SAVE = "Save As";
        const string FILTER = "Cytus II Rating Files (*.c2sr)|*.c2r|JSON Files|*.json|All Files (*.*)|*.*";
        const string DEF_EXT = ".c2sr";
        const string MSG_OPEN_ERROR = "An error occurred while loading the file.";
        const string MSG_SAVE_ERROR = "An error occurred while saving the file.";
    }
}
