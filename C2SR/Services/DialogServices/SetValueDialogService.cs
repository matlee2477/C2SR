using C2SR.Views;

namespace C2SR.Services.DialogServices
{
    class SetValueDialogService : DialogService<SetValueDialogInput, SetValueDialogOutput>
    {
        public SetValueDialogService() : base() { }

        // Methods
        public override bool? ShowDialog(SetValueDialogInput input, out SetValueDialogOutput output)
        {
            SetValueDialog dialog = new() { Owner = Owner };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                output = new()
                {
                    SetsMM = dialog.SetsMM,
                    SetsTP = dialog.SetsTP,
                    SetsMxm = dialog.SetsMxm,
                    IsMM = dialog.IsMM,
                    TP = dialog.TP,
                    IsMxm = dialog.IsMxm
                };
            }
            else
            {
                output = new();
            }

            return result;
        }
    }

    record SetValueDialogInput : IDialogServiceInput
    {
        // The SetValueDialogService class does not require any input parameters.
    }

    record SetValueDialogOutput : IDialogServiceOutput
    {
        public bool SetsMM { get; init; }
        public bool SetsTP { get; init; }
        public bool SetsMxm { get; init; }
        public bool IsMM { get; init; }
        public decimal TP { get; init; }
        public bool IsMxm { get; init; }
    }
}
