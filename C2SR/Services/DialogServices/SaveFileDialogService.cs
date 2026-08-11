using C2SR.Resources;
using Microsoft.Win32;
using static C2SR.App.Constants;

namespace C2SR.Services.DialogServices
{
    class SaveFileDialogService : DialogService<SaveFileDialogServiceInput, SaveFileDialogServiceOutput>
    {
        public SaveFileDialogService() : base() { }

        // Methods
        public override bool? ShowDialog(SaveFileDialogServiceInput input, out SaveFileDialogServiceOutput output)
        {
            SaveFileDialog dialog = new()
            {
                Title = Strings.SaveFileDialog_Title,
                Filter = FILE_FILTER,
                DefaultExt = FILE_DEFAULT_EXT,
                AddExtension = true,
                OverwritePrompt = true
            };

            var result = dialog.ShowDialog(Owner);
            if (result == true)
            {
                output = new() { FileName = dialog.FileName };
            }
            else
            {
                output = null!;
            }

            return result;
        }
    }

    record SaveFileDialogServiceInput : IDialogServiceInput
    {
        // The SaveFileDialogService class does not require any input parameters.
        // It is not responsible for checking if the file already exists or if it can be saved. The caller is responsible for that.
    }

    record SaveFileDialogServiceOutput : IDialogServiceOutput
    {
        public required string FileName { get; init; }
    }
}
