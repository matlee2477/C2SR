using C2SR.ViewModels;
using C2SR.Views;
using System.Collections;

namespace C2SR.Services.DialogServices
{
    class StatisticsDialogService : DialogService<StatisticsDialogInput, StatisticsDialogOutput>
    {
        public StatisticsDialogService() : base() { }

        // Methods
        public override bool? ShowDialog(StatisticsDialogInput input, out StatisticsDialogOutput output)
        {
            StatisticsDialog dialog = new([.. input.Songs.Cast<C2SongViewModel>()]) { Owner = Owner };
            output = new();
            return dialog.ShowDialog();
        }
    }

    record StatisticsDialogInput : IDialogServiceInput
    {
        public required IEnumerable Songs { get; init; }
    }

    record StatisticsDialogOutput : IDialogServiceOutput
    {
        // The StatisticsDialogService class does not produce any output parameters.
    }
}
