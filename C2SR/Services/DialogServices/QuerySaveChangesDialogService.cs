using C2SR.Resources;
using System.Windows;

namespace C2SR.Services.DialogServices
{
    class QuerySaveChangesDialogService : DialogService<QuerySaveChangesDialogServiceInput, QuerySaveChangesDialogServiceOutput>
    {
        public QuerySaveChangesDialogService() : base() { }

        // Methods
        public override bool? ShowDialog(QuerySaveChangesDialogServiceInput input, out QuerySaveChangesDialogServiceOutput output)
        {
            var result = MessageBox.Show(Owner, Strings.MessageBox_QuerySaveChanges, Strings.Title, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            output = new() { DialogResult = result };
            return result switch
            {
                MessageBoxResult.Yes => true,
                _ => false
            };
        }
    }

    record QuerySaveChangesDialogServiceInput : IDialogServiceInput
    {
        // The QuerySaveChangesDialogService class does not require any input parameters.
        // It is not responsible for checking if there are unsaved changes. The caller is responsible for that.
    }

    record QuerySaveChangesDialogServiceOutput : IDialogServiceOutput
    {
        public required MessageBoxResult DialogResult { get; init; }
    }
}
