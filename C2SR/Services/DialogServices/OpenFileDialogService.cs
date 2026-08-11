using C2SR.Resources;
using Microsoft.Win32;
using static C2SR.App.Constants;

namespace C2SR.Services.DialogServices
{
    class OpenFileDialogService : DialogService<OpenFileDialogServiceInput, OpenFileDialogServiceOutput>
    {
        public OpenFileDialogService() : base() { }

        // Methods
        public override bool? ShowDialog(OpenFileDialogServiceInput input, out OpenFileDialogServiceOutput output)
        {
            OpenFileDialog dialog = new()
            {
                Title = Strings.OpenFileDialog_Title,
                Filter = FILE_FILTER,
                DefaultExt = FILE_DEFAULT_EXT,
                AddExtension = true,
                Multiselect = false
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

    record OpenFileDialogServiceInput : IDialogServiceInput
    {
        // The OpenFileDialogService class does not require any input parameters.
    }

    record OpenFileDialogServiceOutput : IDialogServiceOutput
    {
        public required string FileName { get; init; }
    }
}
