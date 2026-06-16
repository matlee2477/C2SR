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
        public SettingsDialogResult ShowSettingsDialog();
        public SetValueDialogResult ShowSetValueDialog();
        public void ShowAboutDialog();
    }

    readonly struct SettingsDialogResult
    {
        public bool DialogResult { get; init; }
        public C2Language Language { get; init; }
        public C2StartAction StartAction { get; init; }
    }

    readonly struct SetValueDialogResult
    {
        public bool DialogResult { get; init; }
        public bool SetsMM { get; init; }
        public bool SetsTP { get; init; }
        public bool SetsMxm { get; init; }
        public bool IsMM { get; init; }
        public decimal TP { get; init; }
        public bool IsMxm { get; init; }
    }
}
